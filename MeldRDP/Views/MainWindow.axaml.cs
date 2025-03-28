namespace MeldRDP.Views {
	using Avalonia.Controls;
	using Avalonia.Input;

	using ReactiveUI;

	public partial class MainWindow : Window {

		public MainWindow() {
			this.InitializeComponent();

			this.KeyBindings.Add(new KeyBinding() {
				Command = ReactiveCommand.Create(this.FocusOnSearch),
				Gesture = new KeyGesture(Key.F3)
			});

			this.KeyBindings.Add(new KeyBinding() {
				Command = ReactiveCommand.Create(this.RefreshConnections),
				Gesture = new KeyGesture(Key.F5)
			});

			var command = ReactiveCommand.Create<int>((num) => this.MainView.ViewModel?.TryConnectByNumber(num));
			for (int i = 0; i < 10; i++) {
				this.KeyBindings.Add(new KeyBinding() {
					Command = command,
					CommandParameter = i,
					Gesture = new KeyGesture(Key.D0 + i, KeyModifiers.Control)
				});

			}
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
