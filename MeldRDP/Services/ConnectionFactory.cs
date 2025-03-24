namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public static class ConnectionFactory {
		public static RdpFileConnectionEndPoint BuildRdp(
			string name,
			ConnectionGroup? group,
			string rdpFilePath
		) {
			var id = Guid.NewGuid().ToString();

			return new RdpFileConnectionEndPoint(
				Id: id,
				Name: name,
				RdpFilepath: rdpFilePath,
				Group: group?.Name ?? "",
				EnableMouseJiggler: false,
				MouseJigglerInterval: null
			);
		}

	}
}