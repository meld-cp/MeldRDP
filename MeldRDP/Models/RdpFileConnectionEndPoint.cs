namespace MeldRDP.Models {
	public record RdpFileConnectionEndPoint(
		string Id,
		string Name,
		string RdpFilepath,
		string Group,
		int? MouseJigglerInterval
	) : IConnectionEndPoint;
}