namespace MeldRDP.Services {
	using System.Collections.Generic;
	using System.IO;

	using Avalonia.Media.Imaging;

	public class DefaultImageProvider(string basePath) : BasePathService(basePath), IImageProvider {

		private readonly Dictionary<string, Bitmap> cache = [];

		public Bitmap? Fetch(string? filename) {

			if (filename == null) {
				return null;
			}

			if (this.cache.TryGetValue(filename, out Bitmap? value)) {
				return value;
			}

			var filePath = this.BuildFilePath(filename);
			if (!File.Exists(filePath)) {
				return null;
			}

			var image = new Bitmap(filePath);
			this.cache[filename] = image;
			return image;
		}
	}
}