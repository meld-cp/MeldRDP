namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class NullRouter : IRouter {
		public void Connect(IConnectionEndPoint endPoint) {
		}

		public void Edit(
			IConnectionEndPoint endPoint,
			bool extendedEdit,
			Action? OnEditingCompleteAction
		) {
		}

		public void ShowMessage(MessageWindowViewModel vm) {

		}
	};
}
