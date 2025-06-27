namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public interface IConnectionRepository {
		event EventHandler? ConnectionsChanged;
		IConnectionEndPoint Create(string name, ConnectionGroup? group);
		void Save(IConnectionEndPoint endPoint);
		void Remove(IConnectionEndPoint endPoint);
		IConnectionEndPoint[] FetchAll();
		IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null);
		ConnectionGroup[] FetchAllGroups();
		void NotifyConnectionsChanged();
	}
}