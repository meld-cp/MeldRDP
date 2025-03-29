namespace MeldRDP.Services {
	using System.Collections.Generic;
	using System.IO;

	using Avalonia.Media.Imaging;

	public class DefaultImageProvider : IImageProvider {
		private readonly string basePath;

		private readonly Dictionary<string, Bitmap> cache = [];

		public DefaultImageProvider(string basePath) {
			this.basePath = basePath;
		}

		private string BuildPath(string imageName) {
			return Path.Combine(this.basePath, imageName);
		}

		public Bitmap? Fetch(string? filename) {

			if (filename == null) {
				return null;
			}

			if (this.cache.TryGetValue(filename, out Bitmap? value)) {
				return value;
			}

			var filePath = this.BuildPath(filename);
			if (!File.Exists(filePath)) {
				return null;
			}

			var image = new Bitmap(filePath);
			this.cache[filename] = image;
			return image;
		}
	}
}