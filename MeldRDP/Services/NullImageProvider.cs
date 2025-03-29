namespace MeldRDP.Services {
	using Avalonia.Media.Imaging;

	public class NullImageProvider : IImageProvider {
		public Bitmap? Fetch(string? filename) {
			return null;
		}
	}
}