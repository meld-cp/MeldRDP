namespace MeldRDP {
	using System.IO;

	using Avalonia;
	using Avalonia.Controls.ApplicationLifetimes;
	using Avalonia.Markup.Xaml;

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

			var dataPath = Path.Combine(appPath, "Data");

			var srvRdp = new RdpExMstscService(appPath);
			var connRepo = new FolderRepository(dataPath);
			this.DesktopRouter = new DesktopRouter(srvRdp: srvRdp, connRepo);

			var mainViewModel = new MainViewModel(
				router: this.DesktopRouter,
				connectionRepo: connRepo
			);

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {

				var mainWin = new MainWindow();
				mainWin.MainView.DataContext = mainViewModel;
				desktop.MainWindow = mainWin;

			} else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {

				singleViewPlatform.MainView = new MainView {
					DataContext = mainViewModel
				};

			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
