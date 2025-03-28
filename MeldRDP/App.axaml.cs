namespace MeldRDP {
	using System;
	using System.IO;

	using Avalonia;
	using Avalonia.Controls;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Markup.Xaml;

	using MeldRDP.Models;
	using MeldRDP.Services;
	using MeldRDP.ViewModels;
	using MeldRDP.Views;

	public partial class App : Application {

		public IRouter? DesktopRouter;

		public override void Initialize() {
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted() {

			// get app path
			var appPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)
				?? throw new System.Exception("Could not determine application path")
			;

			// Load app settings
			var appSettingsRepo = new AppSettingsRepository(appPath);
			var appSettings = appSettingsRepo.Load();


			var dataPath = Path.Combine(appPath, "Data");

			// create services
			var procMon = new ProcessMonitorService(TimeSpan.FromMilliseconds(500));
			var srvRdp = new RdpExMstscService(procMon, appPath);
			var connRepo = new FolderRepository(dataPath);
			this.DesktopRouter = new DesktopRouter(srvRdp: srvRdp, connRepo);

			// create main view model
			var mainViewModel = new MainViewModel(
				router: this.DesktopRouter,
				connectionRepo: connRepo
			);

			if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
				// create main window
				var mainWin = new MainWindow();
				mainWin.MainView.DataContext = mainViewModel;
				desktop.MainWindow = mainWin;

				// Apply app settings
				desktop.MainWindow.Width = appSettings.Width ?? desktop.MainWindow.Width;
				desktop.MainWindow.Height = appSettings.Height ?? desktop.MainWindow.Height;
				if (appSettings.IsMaximized) {
					desktop.MainWindow.WindowState = WindowState.Maximized;
				}

				// Save app settings on close
				desktop.MainWindow.Closing += (sender, e) => {
					AppSettings newAppSettings;
					if (desktop.MainWindow.WindowState == WindowState.Maximized) {
						newAppSettings = appSettings with {
							IsMaximized = true
						};
					} else {
						newAppSettings = appSettings with {
							IsMaximized = false,
							Width = (int?)desktop.MainWindow.Width,
							Height = (int?)desktop.MainWindow.Height
						};
					}
					appSettingsRepo.Save(newAppSettings);
				};

			} else if (this.ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {

				singleViewPlatform.MainView = new MainView {
					DataContext = mainViewModel
				};

			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
