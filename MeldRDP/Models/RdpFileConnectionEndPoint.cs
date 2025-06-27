namespace MeldRDP.Models {
	public record RdpFileConnectionEndPoint(
		string Id,
		string Name,
		string FullAddress,
		string RdpFilepath,
		string Group,
		string? BackgroundImageName,
		bool IsPinned,
		bool EnableMouseJiggler = false,
		int? MouseJigglerInterval = null,
		int? SelectedMonitorsFromId = null,
		int? SelectedMonitorsSpanCount = null
	) : IConnectionEndPoint;
}