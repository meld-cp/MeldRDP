namespace MeldRDP.DesignTime {
	using System;

	using MeldRDP.Models;
	using MeldRDP.Services;
	using MeldRDP.ViewModels;

	public class NullRouter : IRouter {
		public void Connect(IConnectionEndPoint endPoint) {
		}

		public void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? onCompleteAction
		) {
		}

		public void Edit(string editType, IConnectionEndPoint endPoint) {
			throw new NotImplementedException();
		}

		public void OpenSupportTheDevLink() {

		}

		public void SetPinned(IConnectionEndPoint endPoint, bool isPinned) {
			
		}

		public void ShowMessage(MessageWindowViewModel vm) {

		}

		public void TogglePinned(IConnectionEndPoint endPoint) {
			
		}
	};
}
