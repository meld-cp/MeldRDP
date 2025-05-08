namespace MeldRDP.Desktop {
	using System;
	using System.Reactive;
	using System.Threading.Tasks;

	using Avalonia;
	using Avalonia.ReactiveUI;

	using MeldRDP.Services;
	using MeldRDP.ViewModels;

	using ReactiveUI;

	class Program {
		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static void Main(string[] args) {
			TaskScheduler.UnobservedTaskException += (sender, e) => {
				HandleException(e.Exception);
			};

			RxApp.DefaultExceptionHandler = Observer.Create<Exception>(HandleException);

			try {
				BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
			} catch (Exception ex) {
				HandleException(ex);
			} finally {
				Console.WriteLine("Exiting...");
			}
		}

		private static void HandleException(Exception ex) {
			Console.WriteLine($"An error occurred: {ex.Message}\n\n{ex.StackTrace}");

			if (Application.Current is App app && app.DesktopRouter is IRouter router) {
				var msgVm = new MessageWindowViewModel("An error occurred", ex.Message, canCancel: false) {
					Details = ex.StackTrace,
				};
				router.ShowMessage(msgVm);
			}
		}

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp() {
			return AppBuilder
				.Configure<App>()
				.UsePlatformDetect()
				.WithInterFont()
				.LogToTrace()
				.UseReactiveUI()
			;
		}
	}
}
