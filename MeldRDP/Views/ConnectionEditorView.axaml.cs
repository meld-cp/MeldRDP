namespace MeldRDP.Views {
	using Avalonia.ReactiveUI;

	using MeldRDP.ViewModels;

	public partial class ConnectionEditorView : ReactiveUserControl<ConnectionEditorViewModel> {
		public ConnectionEditorView() {
			this.InitializeComponent();
		}
	}
}