namespace MeldRDP.Services {
	using MeldRDP.Models;

	public interface IConnectionRepository {
		IConnectionEndPoint Create(string name, ConnectionGroup? group);
		void Save(IConnectionEndPoint endPoint);
		void Remove(IConnectionEndPoint endPoint);
		IConnectionEndPoint[] FetchAll();
		IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null);
		ConnectionGroup[] FetchAllGroups();
	}
}