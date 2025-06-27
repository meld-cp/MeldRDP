namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	using Avalonia.Controls;
	using Avalonia.Controls.ApplicationLifetimes;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesktopRouter(
		ProcessMonitorService procMon,
		IConnectionRepository connRepo,
		IImageProvider imageProvider
	) : IRouter {
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

		public void OpenInAppEditor(
			IConnectionEndPoint endPoint,
			Action? onCompleteAction = null
		) {

			// Use the default editor
			var editWindow = new ConnectionEditorWindow {
				DataContext = new ConnectionEditorWindowViewModel(connRepo, endPoint, imageProvider)
			};

			editWindow.Closed += (sender, args) => {
				onCompleteAction?.Invoke();
			};

			this.ShowWindowWithOwner(editWindow);

		}

		public void OpenTextEditor(string filepath, Action? onEditingCompleteAction) {
			// TODO: Use the text editor path from settings
			var proc = Process.Start("notepad.exe", [filepath]);
			if (onEditingCompleteAction != null) {
				procMon.MonitorOnExit(proc, onEditingCompleteAction);
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
