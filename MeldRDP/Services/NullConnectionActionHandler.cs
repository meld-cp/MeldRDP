namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public class NullConnectionActionHandler : IConnectionEndPointActionHandler {
		public void Connect(IConnectionEndPoint endPoint) {
			
		}

		public void Edit(string editType, IConnectionEndPoint endPoint, Action? onCompleteAction) {
			
		}

		public void SetPinned(IConnectionEndPoint endPoint, bool isPinned) {
			
		}
	}
}