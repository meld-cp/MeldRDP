namespace MeldRDP.Services {
	using System;

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
				_ = srvRdp.Connect(epRdp.RdpFilepath);
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

		public void Edit(IConnectionEndPoint endPoint, bool extendedEdit) {

			if (extendedEdit && endPoint is RdpFileConnectionEndPoint epRdp) {
				srvRdp.EditRdpFile(epRdp.RdpFilepath);
				return;
			}

			var editWindow = new ConnectionEditorWindow {
				DataContext = new ConnectionEditorWindowViewModel(connRepo, endPoint)
			};

			this.ShowWindowWithOwner(editWindow);
		}


		public void ShowMessage(MessageWindowViewModel vm) {
			var window = new MessageWindow {
				DataContext = vm
			};
			this.ShowWindowWithOwner(window);
		}
	}
}
