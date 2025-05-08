namespace MeldRDP.ViewModels {
	using System;
	using System.Windows.Input;

	using ReactiveUI;

	public class MessageWindowViewModel : ViewModelBase {
		public string Title { get; set; }
		public string Message { get; set; }
		public bool CanCancel { get; set; }

		public string? Details { get; set; }

		public ICommand? OkCommand { get; }
		public ICommand? CancelCommand { get; }

		public event EventHandler<bool>? OnClosedHandler;

		public MessageWindowViewModel(
			string title,
			string message,
			bool canCancel
		) {
			this.Title = title;
			this.Message = message;
			this.CanCancel = canCancel;

			this.OkCommand = ReactiveCommand.Create(() => this.OnClosedHandler?.Invoke(this, true));

			if (this.CanCancel) {
				this.CancelCommand = ReactiveCommand.Create(() => this.OnClosedHandler?.Invoke(this, false));
			}
		}

	}

}
