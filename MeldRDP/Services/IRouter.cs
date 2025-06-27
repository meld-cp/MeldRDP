namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public interface IRouter {
		void Connect(IConnectionEndPoint endPoint);
		void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? onCompleteAction
		);
		void SetPinned(IConnectionEndPoint endPoint, bool isPinned);
		void OpenSupportTheDevLink();
		void ShowMessage(MessageWindowViewModel vm);
	}

}