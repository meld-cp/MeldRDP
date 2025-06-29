﻿namespace MeldRDP.ViewModels {
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Windows.Input;

	using DynamicData;

	using MeldRDP.Models;
	using MeldRDP.Services;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class MainViewModel : ViewModelBase, IActivatableViewModel {

		private readonly IConnectionRepository connectionRepo;
		private readonly IImageProvider backgroundProvider;
		private readonly DefaultConnectionEndPointActionHandler? defaultConnectionEndPointActionHandler;
		private readonly RdpFileConnectionEndPointActionHandler? rdpFileConnectionEndPointActionHandler;

		private IConnectionEndPoint[] allConnections = [];

		[Reactive]
		public string SelectedGroupTitle { get; set; } = "";

		[Reactive]
		public string ConnectionSummary { get; set; } = "";

		[Reactive]
		public string SearchText { get; set; } = "";

		public ObservableCollection<EndPointGroupViewModel> EndPointGroups { get; private set; } = [];

		[Reactive]
		public EndPointGroupViewModel? SelectedGroup { get; set; }

		[Reactive]
		public ObservableCollection<EndPointListItemViewModel> ConnectionEndPoints { get; private set; } = [];


		public ICommand AddConnectionCommand { get; }
		public ICommand RefreshConnectionsCommand { get; }
		public ICommand SupportTheDevCommand { get; }

		public ViewModelActivator Activator { get; } = new();

		public MainViewModel(
			IRouter router,
			IConnectionRepository connectionRepo,
			IImageProvider backgroundProvider,
			DefaultConnectionEndPointActionHandler? defaultConnectionEndPointActionHandler,
			RdpFileConnectionEndPointActionHandler? rdpFileConnectionEndPointActionHandler
		) {
			this.connectionRepo = connectionRepo;
			this.backgroundProvider = backgroundProvider;
			this.defaultConnectionEndPointActionHandler = defaultConnectionEndPointActionHandler;
			this.rdpFileConnectionEndPointActionHandler = rdpFileConnectionEndPointActionHandler;

			this.AddConnectionCommand = ReactiveCommand.Create(this.AddRdpConnection);
			this.RefreshConnectionsCommand = ReactiveCommand.Create(this.RefreshConnections);
			this.SupportTheDevCommand = ReactiveCommand.Create(router.OpenSupportTheDevLink);

			// add an [All] group
			this.EndPointGroups.Add(new(ConnectionGroupType.Everything, name: "[All]"));

			this.connectionRepo.ConnectionsChanged += (sender, args) => {
				// refresh connections when the repository changes
				this.RefreshConnections();
			};

			this.WhenActivated((CompositeDisposable disposables) => {

				this.RefreshConnections();

				this
					.WhenAnyValue(x => x.SelectedGroup)
					.Subscribe(_ => this.RefreshConnections())
				;

				this
					.WhenAnyValue(x => x.SearchText)
					.Skip(1)
					.Subscribe(_ => this.RefreshConnections())
				;

			});

		}

		private EndPointListItemViewModel BuildEndPointListItemViewModel(IConnectionEndPoint endPoint) {

			var result = endPoint switch {

				RdpFileConnectionEndPoint epRdp when this.rdpFileConnectionEndPointActionHandler != null => new EndPointListItemViewModel(
					connectionActionHandler: this.rdpFileConnectionEndPointActionHandler,
					endPoint: endPoint,
					extendedInfo: epRdp.FullAddress,
					extendedEdits: [
						new(
							label: "MSTSC Props.",
							editType: ConnectionEditTypes.Extended
						),
						new(
							label: "Text Editor",
							editType: ConnectionEditTypes.TextEditor
						)
						,
						new(
							label: "Show In File Explorer",
							editType: ConnectionEditTypes.ShowInFileExplorer
						)
					],
					backgroundImage: this.backgroundProvider.Fetch(endPoint.BackgroundImageName),
					isPinned: epRdp.IsPinned
				),

				_ when this.defaultConnectionEndPointActionHandler != null =>
					new EndPointListItemViewModel(
						connectionActionHandler: this.defaultConnectionEndPointActionHandler,
						endPoint: endPoint,
						extendedInfo: "",
						extendedEdits: [],
						backgroundImage: this.backgroundProvider.Fetch(endPoint.BackgroundImageName),
						isPinned: endPoint.IsPinned
					),

				_ => throw new NotSupportedException($"Connection type {endPoint.GetType().Name} is not supported.")
			};

			return result;
		}

		public void RefreshConnections() {

			// fetch the Everything group
			var allGroup = this.EndPointGroups.First(i => i.GroupType == ConnectionGroupType.Everything);

			// make sure there is a group selected
			if (this.SelectedGroup == null) {
				this.SelectedGroup = allGroup; // this will trigger a refresh so return
				return;
			}


			System.Diagnostics.Debug.Assert(this.SelectedGroup != null);
			EndPointGroupViewModel selectedGroup = this.SelectedGroup;

			System.Diagnostics.Debug.WriteLine($"Refreshing connections, {selectedGroup.Name}");
			// fetch all connections
			this.allConnections = this.connectionRepo.FetchAll();

			// check for connections without a group
			var noGroupConnections = this.allConnections.Where(x => string.IsNullOrEmpty(x.Group)).ToArray();

			// remove the noGroup item if all connections have a group
			var noGroup = this.EndPointGroups.FirstOrDefault(i => i.GroupType == ConnectionGroupType.NoGroup);
			if (
				noGroup != null // NoGroup exists in the group list
				&& noGroupConnections.Length == 0 // ...but all connections have a group
				&& selectedGroup.GroupType != ConnectionGroupType.NoGroup // ...and it is not selected
			) {
				this.EndPointGroups.Remove(noGroup);
			} else if (
				noGroupConnections.Length > 0 // there are connections without a group
				&& noGroup == null //...but there isn't a NoGroup item in the group list
			) {
				// add the noGroup item
				noGroup = new(ConnectionGroupType.NoGroup, "[None]");
				this.EndPointGroups.Add(noGroup);
			}

			// fetch all groups
			ConnectionGroup[] groups = this.connectionRepo.FetchAllGroups();

			// add any missing groups from the known list
			foreach (var group in groups) {
				if (this.EndPointGroups.Any(x => x.Name == group.Name)) {
					continue;
				}
				this.EndPointGroups.Add(new EndPointGroupViewModel(group.Type, name: group.Name));
			}

			// remove any groups that no longer exist
			foreach (var group in this.EndPointGroups.ToArray()) {
				if (group.GroupType != ConnectionGroupType.Custom) {
					continue;
				}
				if (groups.All(x => x.Name != group.Name)) {
					_ = this.EndPointGroups.Remove(group);
				}
			}

			// update visible connections
			IConnectionEndPoint[] groupConnections;
			if (selectedGroup.GroupType == ConnectionGroupType.Everything) {
				groupConnections = this.allConnections;
			} else if (selectedGroup.GroupType == ConnectionGroupType.NoGroup) {
				groupConnections = this.connectionRepo.FetchByGroup(string.Empty, this.allConnections);
			} else {
				groupConnections = this.connectionRepo.FetchByGroup(selectedGroup.Name, this.allConnections);
			}

			// create view models for the connections
			var groupConnectionViewModels = groupConnections
				.Select(this.BuildEndPointListItemViewModel)
				.Where(c =>
					string.IsNullOrEmpty(this.SearchText)
					|| c.Name.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.ExtendedInfo.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Group.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Id.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
				)
				.OrderBy(c => c.IsPinned ? 0 : 1)
				.ThenBy(c => c.Name)
				.ToArray()
			;

			this.ConnectionEndPoints.Clear();
			this.ConnectionEndPoints.AddRange(groupConnectionViewModels);

			this.SelectedGroupTitle = selectedGroup.Name;
			this.ConnectionSummary = $"Connections: {groupConnectionViewModels.Length}";

		}

		private void AddRdpConnection() {

			var group = this.SelectedGroup;
			var con = this.connectionRepo.Create(
				name: string.Empty,
				group:
					group?.GroupType == ConnectionGroupType.Custom
					? group.GetGroup()
					: null
			);

			if (con is RdpFileConnectionEndPoint && this.rdpFileConnectionEndPointActionHandler != null) {
				this.rdpFileConnectionEndPointActionHandler.Edit(
					editType: ConnectionEditTypes.Extended,
					endPoint: con,
					onCompleteAction: () => {
						// edit the connection with default editor

						// find con again by id
						var editedCon = this.allConnections.FirstOrDefault(x => x.Id == con.Id);
						if (editedCon == null) {
							return;
						}
						var editedConVm = this.BuildEndPointListItemViewModel(editedCon);
						editedConVm.EditCommand.Execute(null);
					}
				);
				return;
			}

			this.defaultConnectionEndPointActionHandler?.Edit(
				editType: ConnectionEditTypes.InApp,
				endPoint: con,
				onCompleteAction: null
			);

		}

		public void TryConnectByNumber(int number) {
			if (number < 0 || number > 9) {
				return;
			}

			/* Number to index mapping
			 1 => 0
			 2 => 1
			 3 => 2
			 4 => 3
			 5 => 4
			 6 => 5
			 7 => 6
			 8 => 7
			 9 => 8
			 0 => 9
			*/
			var idx = number == 0 ? 9 : number - 1;
			if (idx < 0 || idx >= this.ConnectionEndPoints.Count) {
				return;
			}
			this.ConnectionEndPoints[idx].ConnectCommand.Execute(null);
		}
	}
}
