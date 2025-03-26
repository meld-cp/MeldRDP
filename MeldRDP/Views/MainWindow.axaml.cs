namespace MeldRDP.Views {
	using Avalonia.Controls;

	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Activated(object? sender, System.EventArgs e) {
			this.MainView.SearchText.Focus();
			this.MainView.SearchText.SelectAll();
		}
	}
}
