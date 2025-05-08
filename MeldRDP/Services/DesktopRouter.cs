namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	using Avalonia.Controls;
	using Avalonia.Controls.ApplicationLifetimes;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesktopRouter : IRouter {
		private readonly IRdpMstscService srvRdp;
		private readonly IConnectionRepository connRepo;
		private readonly IImageProvider imageProvider;
		private readonly IRdpFileProcessor? rdpFileProcesser;

		public DesktopRouter(
			IRdpMstscService srvRdp,
			IConnectionRepository connRepo,
			IImageProvider imageProvider,
			IRdpFileProcessor? rdpFileProcesser
		) {
			this.srvRdp = srvRdp;
			this.connRepo = connRepo;
			this.imageProvider = imageProvider;
			this.rdpFileProcesser = rdpFileProcesser;
		}

		public void Connect(IConnectionEndPoint endPoint) {

			if (endPoint is RdpFileConnectionEndPoint epRdp) {

				this.rdpFileProcesser?.Process(epRdp.RdpFilepath);

				_ = this.srvRdp.Connect(epRdp.RdpFilepath, null);

				return;
			}

			throw new NotImplementedException();

		}

		private void ShowWindowWithOwner(Window win) {
			var mainWindow = (Avalonia.Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

			if (mainWindow == null) {
				win.Show();
			} else {
				win.Show(mainWindow);
			}
		}

		public Action? RunOnMainThread(Action? action) {
			if (action == null) {
				return null;
			}
			return () => Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(action);
		}

		public void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? OnEditingCompleteAction
		) {

			if (endPoint is RdpFileConnectionEndPoint epRdp) {
				if (editType == DefaultEditTypes.Extended) {
					this.srvRdp.EditRdpFile(epRdp.RdpFilepath, this.RunOnMainThread(OnEditingCompleteAction));
					return;
				}
			}

			// Use the default editor
			var editWindow = new ConnectionEditorWindow {
				DataContext = new ConnectionEditorWindowViewModel(this.connRepo, endPoint, this.imageProvider)
			};

			if (OnEditingCompleteAction != null) {
				editWindow.Closed += (sender, args) => {
					OnEditingCompleteAction();
				};
			}


			this.ShowWindowWithOwner(editWindow);
		}


		public void ShowMessage(MessageWindowViewModel vm) {
			var window = new MessageWindow {
				DataContext = vm
			};
			vm.OnClosedHandler += (sender, result) => {
				window.Close();
			};
			this.ShowWindowWithOwner(window);
		}

		public void OpenSupportTheDevLink() {
			Process.Start(new ProcessStartInfo {
				UseShellExecute = true,
				FileName = "https://buymeacoffee.com/cleon"
			});
		}
	}
}
