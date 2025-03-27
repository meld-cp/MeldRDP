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

		public DesktopRouter(IRdpMstscService srvRdp, IConnectionRepository connRepo) {
			this.srvRdp = srvRdp;
			this.connRepo = connRepo;
		}

		public void Connect(IConnectionEndPoint endPoint) {

			if (endPoint is RdpFileConnectionEndPoint epRdp) {
				_ = srvRdp.Connect(epRdp.RdpFilepath, null);
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
			IConnectionEndPoint endPoint,
			bool extendedEdit,
			Action? OnEditingCompleteAction
		) {

			if (extendedEdit && endPoint is RdpFileConnectionEndPoint epRdp) {
				srvRdp.EditRdpFile(epRdp.RdpFilepath, RunOnMainThread(OnEditingCompleteAction));
				return;
			}

			var editWindow = new ConnectionEditorWindow {
				DataContext = new ConnectionEditorWindowViewModel(connRepo, endPoint)
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
