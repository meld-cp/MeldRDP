namespace MeldRDP.Models {
	public record AppSettings(
		bool IsMaximized,
		int? Width,
		int? Height
	);
}
