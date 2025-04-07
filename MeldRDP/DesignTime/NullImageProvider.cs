namespace MeldRDP.DesignTime {
	using Avalonia.Media.Imaging;

	using MeldRDP.Services;

	public class NullImageProvider : IImageProvider {
		public Bitmap? Fetch(string? filename) {
			return null;
		}
	}
}