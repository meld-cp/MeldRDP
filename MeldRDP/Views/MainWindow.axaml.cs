namespace MeldRDP.Views {
	using Avalonia.Controls;

	using MeldRDP.ViewModels;

	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Activated(object? sender, System.EventArgs e) {
			var vm = this.MainView.DataContext as MainViewModel;
			if (vm == null) {
				return;
			}
			this.MainView.SearchText.Focus();
			this.MainView.SearchText.SelectAll();
			vm.RefreshConnections();
		}
	}
}
