namespace MeldRDP.Models {
	public record RdpFileConnectionEndPoint(
		string Id,
		string Name,
		string FullAddress,
		string RdpFilepath,
		string Group,
		bool EnableMouseJiggler = false,
		int? MouseJigglerInterval = null,
		int? SelectedMonitorsFromId = null,
		int? SelectedMonitorsSpanCount = null
	) : IConnectionEndPoint;
}