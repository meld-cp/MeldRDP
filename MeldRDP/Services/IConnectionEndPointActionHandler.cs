namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public interface IConnectionEndPointActionHandler {
		void Connect(IConnectionEndPoint endPoint);
		void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? onCompleteAction
		);
		void SetPinned(IConnectionEndPoint endPoint, bool isPinned);

	}
}