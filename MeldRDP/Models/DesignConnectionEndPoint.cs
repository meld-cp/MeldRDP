namespace MeldRDP.Models {
	using System;

	public record DesignConnectionEndPoint : IConnectionEndPoint {
		public string Id { get; } = Guid.NewGuid().ToString();
		public string Name { get; } = "Connection Name";
		public string Group { get; } = "Group Name";
		public string? BackgroundImageName { get; }
	}


}
