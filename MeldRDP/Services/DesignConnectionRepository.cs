namespace MeldRDP.Services {
	using System;
	using System.Linq;

	using MeldRDP.Models;

	public class DesignConnectionRepository : IConnectionRepository {
		public IConnectionEndPoint Create(string name, ConnectionGroup? group) {
			throw new NotImplementedException();
		}

		public void Save(IConnectionEndPoint endPoint) {
			throw new System.NotImplementedException();
		}

		public IConnectionEndPoint[] FetchAll() {
			return [.. Enumerable.Range(1, 10).Select(num =>
				new DesignConnectionEndPoint()
			)];
		}

		public IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null) {
			return connections ?? this.FetchAll();
		}

		public void Remove(IConnectionEndPoint endPoint) {
			throw new System.NotImplementedException();
		}

		public void Update(IConnectionEndPoint endPoint) {
			throw new System.NotImplementedException();
		}

		public T Create<T>() where T : IConnectionEndPoint {
			throw new NotImplementedException();
		}

		public ConnectionGroup[] FetchAllGroups() {
			return [.. Enumerable.Range(1, 10).Select(num => new ConnectionGroup(ConnectionGroupType.Custom, $"Group {num}"))];
		}


	};


}
