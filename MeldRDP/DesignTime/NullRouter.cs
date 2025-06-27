namespace MeldRDP.DesignTime {
	using System;

	using MeldRDP.Models;
	using MeldRDP.Services;
	using MeldRDP.ViewModels;

	public class NullRouter : IRouter {

		public void OpenInAppEditor(IConnectionEndPoint endPoint, Action? onCompleteAction = null) {

		}

		public void OpenSupportTheDevLink() {

		}

		public void OpenTextEditor(string filepath, Action? onEditingCompleteAction) {

		}

		public Action? RunOnMainThread(Action? action) {
			return action;
		}

		public void ShowMessage(MessageWindowViewModel vm) {

		}
	};
}
