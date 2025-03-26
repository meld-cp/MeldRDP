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

		[Reactive]
		public ObservableCollection<EndPointListItemViewModel> ConnectionEndPoints { get; set; } = [];


		public ICommand AddConnectionCommand { get; }
		public ICommand RefreshConnectionsCommand { get; }

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

		public void RefreshConnections() {

			if (this.EndPointGroups.Count == 0) {
				this.EndPointGroups.Add(allGroup);
				this.SelectedGroup = allGroup;
			}

			var allConnections = this.connectionRepo.FetchAll();
			var noGroupConnections = allConnections.Where(x => string.IsNullOrEmpty(x.Group)).ToArray();
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
					this.EndPointGroups.Remove(group);
				}
			}

			// update visible connections
			IConnectionEndPoint[] groupConnections;
			var selectedGroup = this.SelectedGroup ?? allGroup;
			if (selectedGroup == allGroup) {
				groupConnections = allConnections;
			} else if (selectedGroup == noGroup) {
				groupConnections = this.connectionRepo.FetchByGroup(string.Empty, allConnections);
			} else {
				groupConnections = this.connectionRepo.FetchByGroup(selectedGroup.Name, allConnections);
			}

			var groupConnectionViewModels = groupConnections
				.Where(c =>
					string.IsNullOrEmpty(this.SearchText)
					|| c.Name.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Group.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
					|| c.Id.Contains(this.SearchText, StringComparison.InvariantCultureIgnoreCase)
				)
				.Select(x => new EndPointListItemViewModel(
					router: this.router,
					endPoint: x,
					OnEditingCompleteAction: this.RefreshConnections
				))
			;

			this.ConnectionEndPoints.Clear();
			this.ConnectionEndPoints.AddRange(groupConnectionViewModels);

			SelectedGroupTitle = selectedGroup.Name ?? $"Connections: {groupConnections.Length}";
			ConnectionSummary = $"Connections: {groupConnections.Length}";


		}

		private void AddRdpConnection() {

			var group = this.SelectedGroup ?? allGroup;

			var con = connectionRepo.Create(name: "", group: group.IsVirtual ? null : group.GetGroup());

			this.RefreshConnections();

			this.router.Edit(
				endPoint: con,
				extendedEdit: true,
				OnEditingCompleteAction: this.RefreshConnections
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
