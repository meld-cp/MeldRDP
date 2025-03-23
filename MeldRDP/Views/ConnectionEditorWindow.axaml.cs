namespace MeldRDP {
	using System;
	using System.Reactive.Linq;

	using Avalonia.ReactiveUI;

	using MeldRDP.ViewModels;

	using ReactiveUI;

	public partial class ConnectionEditorWindow : ReactiveWindow<ConnectionEditorWindowViewModel> {
		public ConnectionEditorWindow() {
			InitializeComponent();

			this.WhenActivated(disposables => {

				if (this.ViewModel == null) {
					return;
				}

				this.ViewModel.OnClose += (sender, args) => {
					this.Close();
				};

			});

		}
	}
}