namespace MeldRDP.DesignTime {
	using System;

	using MeldRDP.Models;

	public record DesignConnectionEndPoint : IConnectionEndPoint {
		public string Id { get; } = Guid.NewGuid().ToString();
		public string Name { get; } = "Connection Name";
		public string Group { get; } = "Group Name";
		public string? BackgroundImageName { get; }
		public bool IsPinned { get; set; }
	}


}
