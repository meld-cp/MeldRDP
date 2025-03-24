﻿namespace MeldRDP.ViewModels {
	using System;
	using System.Windows.Input;

	using MeldRDP.Models;
	using MeldRDP.Services;

	using ReactiveUI;

	public class ConnectionEditorWindowViewModel : ViewModelBase {
		private readonly IConnectionRepository connRepo;
		private readonly IConnectionEndPoint endpoint;

		public ConnectionEditorViewModel ConnectionEditorViewModel { get; }
		public RdpConnectionEditorViewModel? RdpConnectionEditorViewModel { get; }

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand DeleteCommand { get; }

		public event EventHandler? OnClose;

		public ConnectionEditorWindowViewModel(
			IConnectionRepository connRepo,
			IConnectionEndPoint endpoint
		) {
			this.connRepo = connRepo;
			this.endpoint = endpoint;

			this.ConnectionEditorViewModel = new ConnectionEditorViewModel(endpoint);
			if (endpoint is RdpFileConnectionEndPoint epRdp) {
				this.RdpConnectionEditorViewModel = new RdpConnectionEditorViewModel(epRdp);
			}


			this.SaveCommand = ReactiveCommand.Create(this.Save);
			this.CancelCommand = ReactiveCommand.Create(this.Cancel);
			this.DeleteCommand = ReactiveCommand.Create(this.Delete);
		}

		private void Save() {
			this.connRepo.Save(this.BuildConnection());
			this.OnClose?.Invoke(this, EventArgs.Empty);
		}

		private IConnectionEndPoint BuildConnection() {
			if (endpoint is RdpFileConnectionEndPoint epRdp) {
				return epRdp with {
					Name = this.ConnectionEditorViewModel.Name,
					Group = this.ConnectionEditorViewModel.Group,
					EnableMouseJiggler = this.RdpConnectionEditorViewModel?.EnableMouseJiggler ?? false,
					MouseJigglerInterval = this.RdpConnectionEditorViewModel?.MouseJigglerInterval
				};
			}
			throw new NotImplementedException();
		}

		private void Delete() {
			connRepo.Remove(endpoint);
			this.OnClose?.Invoke(this, EventArgs.Empty);
		}

		private void Cancel() {
			this.OnClose?.Invoke(this, EventArgs.Empty);
		}

	}
}