namespace MeldRDP.ViewModels {
	using MeldRDP.Models;

	using ReactiveUI.Fody.Helpers;

	public class ConnectionEditorViewModel : ViewModelBase {

		[Reactive]
		public string Id { get; set; }

		[Reactive]
		public string Name { get; set; }

		[Reactive]
		public string Group { get; set; }

		public ConnectionEditorViewModel(IConnectionEndPoint endpoint) {
			this.Id = endpoint.Id;
			this.Name = endpoint.Name;
			this.Group = endpoint.Group;
		}
	}

}
