namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public interface IRouter {
		void Connect(IConnectionEndPoint endPoint);
		void Edit(
			IConnectionEndPoint endPoint,
			bool extendedEdit,
			Action? OnEditingCompleteAction
		);
		void ShowMessage(MessageWindowViewModel vm);
	}

}