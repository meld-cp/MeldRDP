namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public interface IRouter {
		Action? RunOnMainThread(Action? action);

		void OpenInAppEditor(
			IConnectionEndPoint endPoint,
			Action? onCompleteAction = null
		);

		void OpenTextEditor(
			string filepath,
			Action? onEditingCompleteAction
		);

		void OpenSupportTheDevLink();
		void ShowMessage(MessageWindowViewModel vm);
	}
}