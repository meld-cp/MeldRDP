namespace MeldRDP.DesignTime {
	using System;

	using MeldRDP.Models;
	using MeldRDP.Services;

	public class NullConnectionRepository : IConnectionRepository {
		public event EventHandler? ConnectionsChanged;

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

		public void NotifyConnectionsChanged() {
			
		}

		public void Remove(IConnectionEndPoint endPoint) {
		}

		public void Save(IConnectionEndPoint endPoint) {
		}
	}
}
