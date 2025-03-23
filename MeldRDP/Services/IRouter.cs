namespace MeldRDP.Services {
	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public interface IRouter {
		void Connect(IConnectionEndPoint endPoint);
		void Edit(IConnectionEndPoint endPoint, bool extendedEdit);
		void ShowMessage(MessageWindowViewModel vm);
	}

}