namespace MeldRDP.Views {
	using Avalonia.Controls;
	using Avalonia.Input;

	using ReactiveUI;

	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();

			this.KeyBindings.Add(new KeyBinding() {
				Command = ReactiveCommand.Create(FocusOnSearch),
				Gesture = new KeyGesture(Key.F3)
			});

			this.KeyBindings.Add(new KeyBinding() {
				Command = ReactiveCommand.Create(RefreshConnections),
				Gesture = new KeyGesture(Key.F5)
			});

		}
		private void Window_Activated(object? sender, System.EventArgs e) {
			this.FocusOnSearch();
		}

		private void RefreshConnections() {
			this.MainView.ViewModel?.RefreshConnections();
		}

		private void FocusOnSearch() {
			this.MainView.SearchText.Focus();
			this.MainView.SearchText.SelectAll();
		}

	}
}
