namespace MeldRDP.Views {
	using Avalonia.ReactiveUI;

	using MeldRDP.ViewModels;

	public partial class EndPointListItemView : ReactiveUserControl<EndPointListItemViewModel> {
		public EndPointListItemView() {
			InitializeComponent();
		}
	}
}