namespace MeldRDP.ViewModels {
	using System.Windows.Input;

	public class MessageWindowViewModel : ViewModelBase {

		public string Title { get; set; }
		public string Message { get; set; }
		public string? Details { get; set; }

		public ICommand? OkCommand { get; }
		public ICommand? CancelCommand { get; }

		public MessageWindowViewModel(
			string title,
			string message
		) {
			Title = title;
			Message = message;
		}

	}

}
