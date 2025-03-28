namespace MeldRDP.ViewModels {
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

		private readonly IRouter router;
		private readonly IConnectionRepository connectionRepo;

		[Reactive]
		public string SelectedGroupTitle { get; set; } = "";

		[Reactive]
		public string ConnectionSummary { get; set; } = "";

		[Reactive]
		public string SearchText { get; set; } = "";


		public ObservableCollection<EndPointGroupViewModel> EndPointGroups { get; private set; } = [];

		[Reactive]
		public EndPointGroupViewModel? SelectedGroup { get; set; }


		private IConnectionEndPoint[] allConnections = [];

		[Reactive]
		public ObservableCollection<EndPointListItemViewModel> ConnectionEndPoints { get; private set; } = [];


		public ICommand AddConnectionCommand { get; }
		public ICommand RefreshConnectionsCommand { get; }
		public ICommand SupportTheDevCommand { get; }

		public ViewModelActivator Activator { get; } = new();

		private static readonly EndPointGroupViewModel allGroup = new("[All]", true);
		private static readonly EndPointGroupViewModel noGroup = new("[None]", true);

		public MainViewModel(
			IRouter router,
			IConnectionRepository connectionRepo
		) {
			this.router = router;
			this.connectionRepo = connectionRepo;

			this.AddConnectionCommand = ReactiveCommand.Create(this.AddRdpConnection);
			this.RefreshConnectionsCommand = ReactiveCommand.Create(this.RefreshConnections);
			this.SupportTheDevCommand = ReactiveCommand.Create(router.OpenSupportTheDevLink);

			this.WhenActivated((CompositeDisposable disposables) => {

				this.RefreshConnections();

				this
					.WhenAnyValue(x => x.SelectedGroup)
					.Skip(1)
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
			return new EndPointListItemViewModel(
				router: this.router,
				endPoint: endPoint,
				extendedInfo: endPoint is RdpFileConnectionEndPoint epRdp ? epRdp.FullAddress : "",
				OnEditingCompleteAction: this.RefreshConnections
			);
		}

		public void RefreshConnections() {

			//System.Diagnostics.Debug.WriteLine("Refreshing connections");

			if (this.EndPointGroups.Count == 0) {
				this.EndPointGroups.Add(allGroup);
				this.SelectedGroup = allGroup;
			}

			this.allConnections = this.connectionRepo.FetchAll();
			var noGroupConnections = this.allConnections.Where(x => string.IsNullOrEmpty(x.Group)).ToArray();
			if (noGroupConnections.Length > 0 && !this.EndPointGroups.Contains(noGroup)) {
				this.EndPointGroups.Add(noGroup);
			}

			ConnectionGroup[] groups = this.connectionRepo.FetchAllGroups();

			//TODO: add and remove groups below can be optimized

			// add any new groups
			foreach (var group in groups) {
				if (this.EndPointGroups.Any(x => x.Name == group.Name)) {
					continue;
				}
				this.EndPointGroups.Add(new EndPointGroupViewModel(name: group.Name, isVirtual: false));
			}

			// remove any groups that no longer exist
			foreach (var group in this.EndPointGroups.ToArray()) {
				if (group == allGroup) {
					continue;
				}
				if (group == noGroup) {
					continue;
				}
				if (groups.All(x => x.Name != group.Name)) {
					_ = this.EndPointGroups.Remove(group);
				}
			}

			// update visible connections
			IConnectionEndPoint[] groupConnections;
			var selectedGroup = this.SelectedGroup ?? allGroup;
			if (selectedGroup == allGroup) {
				groupConnections = this.allConnections;
			} else if (selectedGroup == noGroup) {
				groupConnections = this.connectionRepo.FetchByGroup(string.Empty, this.allConnections);
			} else {
				groupConnections = this.connectionRepo.FetchByGroup(selectedGroup.Name, this.allConnections);
			}

			var groupConnectionViewModels = groupConnections
				.Select(this.BuildEndPointListItemViewModel)
				.Where(c =>
					string.IsNullOrEmpty(this.SearchText)
					|| c.Name.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.ExtendedInfo.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Group.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Id.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
				)
				.ToArray()
			;

			this.ConnectionEndPoints.Clear();
			this.ConnectionEndPoints.AddRange(groupConnectionViewModels);

			this.SelectedGroupTitle = selectedGroup.Name ?? $"Connections: {groupConnectionViewModels.Length}";
			this.ConnectionSummary = $"Connections: {groupConnectionViewModels.Length}";


		}

		private void AddRdpConnection() {


			var group = this.SelectedGroup ?? allGroup;
			var con = this.connectionRepo.Create(name: "", group: group.IsVirtual ? null : group.GetGroup());

			this.ConnectionEndPoints.Insert(0, this.BuildEndPointListItemViewModel(con));

			this.router.Edit(
				endPoint: con,
				extendedEdit: true,
				OnEditingCompleteAction: () => {
					// refresh connections after editing
					this.RefreshConnections();

					// non-extended edit the connection

					// find con again by id
					var editedCon = this.allConnections.FirstOrDefault(x => x.Id == con.Id);
					if (editedCon == null) {
						return;
					}
					var editedConVm = this.BuildEndPointListItemViewModel(editedCon);
					editedConVm.EditCommand.Execute(null);
				}
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
