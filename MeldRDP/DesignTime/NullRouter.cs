﻿namespace MeldRDP.DesignTime {
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
			Action? OnEditingCompleteAction
		) {
		}

		public void OpenSupportTheDevLink() {

		}

		public void ShowMessage(MessageWindowViewModel vm) {

		}
	};
}
