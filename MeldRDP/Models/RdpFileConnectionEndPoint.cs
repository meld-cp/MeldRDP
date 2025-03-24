﻿namespace MeldRDP.Models {
	public record RdpFileConnectionEndPoint(
		string Id,
		string Name,
		string RdpFilepath,
		string Group,
		bool EnableMouseJiggler = false,
		int? MouseJigglerInterval = null
	) : IConnectionEndPoint;
}