namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	using Avalonia.Controls;
	using Avalonia.Controls.ApplicationLifetimes;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesktopRouter : IRouter {
		private readonly ProcessMonitorService procMon;
		private readonly IRdpMstscService srvRdp;
		private readonly IConnectionRepository connRepo;
		private readonly IImageProvider imageProvider;
		private readonly IRdpFileProcessor? rdpFileProcesser;

		public DesktopRouter(
			ProcessMonitorService procMon,
			IRdpMstscService srvRdp,
			IConnectionRepository connRepo,
			IImageProvider imageProvider,
			IRdpFileProcessor? rdpFileProcesser
		) {
			this.procMon = procMon;
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

			switch (editType) {
				case ConnectionEditTypes.InApp:
					// Use the default editor
					var editWindow = new ConnectionEditorWindow {
						DataContext = new ConnectionEditorWindowViewModel(this.connRepo, endPoint, this.imageProvider)
					};

					if (OnEditingCompleteAction != null) {
						editWindow.Closed += (sender, args) => {
							OnEditingCompleteAction?.Invoke();
						};
					}

					this.ShowWindowWithOwner(editWindow);
					return;


				case ConnectionEditTypes.Extended: {
						if (endPoint is RdpFileConnectionEndPoint epRdp) {
							this.srvRdp.EditRdpFile(
								path: epRdp.RdpFilepath,
								OnExitAction: this.RunOnMainThread(OnEditingCompleteAction)
							);
							return;
						}

						break;
					}

				case ConnectionEditTypes.TextEditor: {
						if (endPoint is RdpFileConnectionEndPoint epRdp) {
							this.OpenTextEditor(epRdp.RdpFilepath, this.RunOnMainThread(OnEditingCompleteAction));
							return;
						}

						break;
					}

			}

			throw new NotImplementedException($"Edit type '{editType}' not implemented for '{endPoint.GetType().Name}'");

		}

		private void OpenTextEditor(string filepath, Action? onEditingCompleteAction) {
			// TODO: Use the text editor path from settings
			var proc = Process.Start("notepad.exe", [filepath]);
			if (onEditingCompleteAction != null) {
				this.procMon.MonitorOnExit(proc, onEditingCompleteAction);
			}
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
