namespace MeldRDP.Models {
	public interface IConnectionEndPoint {
		string Id { get; }
		string Name { get; }
		string Group { get; }
		string? BackgroundImageName { get; }
	};
}
