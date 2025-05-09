﻿namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public interface IRouter {
		void Connect(IConnectionEndPoint endPoint);
		void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? OnEditingCompleteAction
		);
		void OpenSupportTheDevLink();
		void ShowMessage(MessageWindowViewModel vm);
	}

}