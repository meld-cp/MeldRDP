namespace MeldRDP.Services {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;

	using MeldRDP.Models;

	public class FolderRepository : IConnectionRepository {
		private readonly string basePath;

		public FolderRepository(string basePath) {
			this.basePath = basePath;
		}

		private void EnsureDirectoryExists() {
			if (!Directory.Exists(basePath)) {
				Directory.CreateDirectory(basePath);
			}
		}

		private string BuildDataConnectionPath(IConnectionEndPoint endPoint) {
			if (endPoint is RdpFileConnectionEndPoint rdpEndPoint) {
				return Path.Combine(basePath, $"{rdpEndPoint.Id}.rdp");
			}
			throw new NotImplementedException();
		}

		public void Save(IConnectionEndPoint endPoint) {
			EnsureDirectoryExists();

			if (endPoint is not RdpFileConnectionEndPoint rdpEndPoint) {
				throw new NotImplementedException();
			}

			var rdpFile = new RdpFormatFile(rdpEndPoint.RdpFilepath);

			rdpFile.SetValue(KnownRdpFormatKeys.MeldName, rdpEndPoint.Name);
			rdpFile.SetValue(KnownRdpFormatKeys.MeldGroup, rdpEndPoint.Group);
			rdpFile.SetValue(KnownRdpFormatKeys.EnableMouseJiggler, rdpEndPoint.EnableMouseJiggler ? 1 : 0);
			rdpFile.SetValue(KnownRdpFormatKeys.MouseJigglerInterval, rdpEndPoint.MouseJigglerInterval);

			var destPath = BuildDataConnectionPath(endPoint);

			rdpFile.SaveAs(destPath);

		}

		public IConnectionEndPoint[] FetchAll() {
			if (!Directory.Exists(basePath)) {
				return [];
			}

			var result = new List<IConnectionEndPoint>();

			// fetch rdp files
			var rdpFiles = Directory
				.EnumerateFiles(basePath, "*.rdp", SearchOption.TopDirectoryOnly)
				.Select(filePath => new RdpFormatFile(filePath))
				.ToArray()
			;

			Debug.Assert(rdpFiles != null);

			if (rdpFiles.Length == 0) {
				return [];
			}

			// deserialize data files
			foreach (var rdpFile in rdpFiles) {

				Debug.Assert(rdpFile != null);

				var id = Path.GetFileNameWithoutExtension(rdpFile.Path);
				if (id == null) {
					continue;
				}

				var rdpEp = new RdpFileConnectionEndPoint(
					Id: id,
					Name: rdpFile.GetStringValue(KnownRdpFormatKeys.MeldName) ?? "",
					RdpFilepath: rdpFile.Path,
					Group: rdpFile.GetStringValue(KnownRdpFormatKeys.MeldGroup) ?? "",
					EnableMouseJiggler: rdpFile.GetIntValue(KnownRdpFormatKeys.EnableMouseJiggler) == 1,
					MouseJigglerInterval: rdpFile.GetIntValue(KnownRdpFormatKeys.MouseJigglerInterval)
				);

				result.Add(rdpEp);

			}

			return [.. result];
		}

		public IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null) {
			connections ??= this.FetchAll();
			return [.. connections.Where(ep => ep.Group == groupName)];
		}

		public void Remove(IConnectionEndPoint endPoint) {
			var destPath = BuildDataConnectionPath(endPoint);
			if (File.Exists(destPath)) {
				File.Delete(destPath);
			}
		}

		public ConnectionGroup[] FetchAllGroups() {
			return [.. this.FetchAll()
				.Select(ep => ep.Group)
				.Distinct()
				.Where(groupName => !string.IsNullOrWhiteSpace(groupName))
				.Select(groupName => new ConnectionGroup(groupName))
			];
		}
	}

}
