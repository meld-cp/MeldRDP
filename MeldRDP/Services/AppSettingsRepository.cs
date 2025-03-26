namespace MeldRDP.Services {
	using System.IO;

	using MeldRDP.Models;

	public class AppSettingsRepository(string basePath) {
		private const string KEY_IS_MAXIMISED = "is maximised";
		private const string KEY_WITDH = "width";
		private const string KEY_HEIGHT = "height";

		private void EnsureDirectoryExists() {
			if (!Directory.Exists(basePath)) {
				Directory.CreateDirectory(basePath);
			}
		}

		private string BuildFilePath() {
			return Path.Combine(basePath, "appsettings.data");
		}

		public void Save(AppSettings settings) {
			EnsureDirectoryExists();

			var path = BuildFilePath();

			var rdpFile = new RdpFormatFile(path);
			rdpFile.SetValue(KEY_IS_MAXIMISED, settings.IsMaximized ? 1 : 0);
			rdpFile.SetValue(KEY_WITDH, settings.Width);
			rdpFile.SetValue(KEY_HEIGHT, settings.Height);
			rdpFile.Save();
		}


		public AppSettings Load() {
			var path = BuildFilePath();
			if (!File.Exists(path)) {
				return new AppSettings(IsMaximized: false, Width: null, Height: null);
			}

			var rdpFile = new RdpFormatFile(path);
			return new AppSettings(
				IsMaximized: rdpFile.GetIntValue(KEY_IS_MAXIMISED) == 1,
				Width: rdpFile.GetIntValue(KEY_WITDH),
				Height: rdpFile.GetIntValue(KEY_HEIGHT)
			);
		}
	}
}
