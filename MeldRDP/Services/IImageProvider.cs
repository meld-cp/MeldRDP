namespace MeldRDP.Services {
	using Avalonia.Media.Imaging;

	public interface IImageProvider {
		Bitmap? Fetch(string? filename);
	}

}