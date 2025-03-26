namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public static class ConnectionFactory {

		public static string BuildNewId() {
			return Guid.NewGuid().ToString("N");
		}

		public static RdpFileConnectionEndPoint BuildRdp(
			string id,
			string name,
			ConnectionGroup? group,
			string rdpFilePath
		) {
			return new RdpFileConnectionEndPoint(
				Id: id,
				Name: name,
				FullAddress: "",
				RdpFilepath: rdpFilePath,
				Group: group?.Name ?? "",
				EnableMouseJiggler: false,
				MouseJigglerInterval: null
			);
		}

	}
}