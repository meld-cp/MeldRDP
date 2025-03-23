namespace MeldRDP.Services {
	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class NullRouter : IRouter {
		public void Connect(IConnectionEndPoint endPoint) {
		}

		public void Edit(IConnectionEndPoint endPoint, bool extendedEdit) {
		}

		public void ShowMessage(MessageWindowViewModel vm) {

		}
	};
}
