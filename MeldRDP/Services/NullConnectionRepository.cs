namespace MeldRDP.Services {
	using MeldRDP.Models;

	public class NullConnectionRepository : IConnectionRepository {
		public IConnectionEndPoint Create(string name, ConnectionGroup? group) {
			return null!;
		}

		public IConnectionEndPoint[] FetchAll() {
			return [];
		}

		public ConnectionGroup[] FetchAllGroups() {
			return [];
		}

		public IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null) {
			return [];
		}

		public void Remove(IConnectionEndPoint endPoint) {
		}

		public void Save(IConnectionEndPoint endPoint) {
		}
	}
}
