namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public class DefaultConnectionEndPointActionHandler : IConnectionEndPointActionHandler {
		public void Connect(IConnectionEndPoint endPoint) {
			throw new NotImplementedException();
		}

		public void Edit(string editType, IConnectionEndPoint endPoint, Action? onCompleteAction) {
			throw new NotImplementedException();
		}

		public void SetPinned(IConnectionEndPoint endPoint, bool isPinned) {
			throw new NotImplementedException();
		}
	}
}