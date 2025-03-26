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

		private string BuildDataConnectionPath(string id) {
			return Path.Combine(basePath, $"{id}.rdp");
		}

		private string BuildDataConnectionPath(IConnectionEndPoint endPoint) {
			if (endPoint is RdpFileConnectionEndPoint rdpEndPoint) {
				return BuildDataConnectionPath(rdpEndPoint.Id);
			}
			throw new NotImplementedException();
		}

		public IConnectionEndPoint Create(string name, ConnectionGroup? group) {
			EnsureDirectoryExists();

			var id = ConnectionFactory.BuildNewId();

			var con = ConnectionFactory.BuildRdp(
				id: id,
				name: name,
				group: group,
				rdpFilePath: BuildDataConnectionPath(id)
			);

			this.Save(con);

			return con;
		}

		public void Save(IConnectionEndPoint endPoint) {
			EnsureDirectoryExists();

			if (endPoint is not RdpFileConnectionEndPoint rdpEndPoint) {
				throw new NotImplementedException();
			}

			var rdpFile = new RdpFormatFile(rdpEndPoint.RdpFilepath);
			rdpFile.SetValue(KnownRdpFormatKeys.FullAddress, rdpEndPoint.FullAddress);

			rdpFile.SetValue(KnownRdpFormatKeys.MeldName, rdpEndPoint.Name);
			rdpFile.SetValue(KnownRdpFormatKeys.MeldGroup, rdpEndPoint.Group);
			rdpFile.SetValue(KnownRdpFormatKeys.EnableMouseJiggler, rdpEndPoint.EnableMouseJiggler ? 1 : 0);
			rdpFile.SetValue(KnownRdpFormatKeys.MouseJigglerInterval, rdpEndPoint.MouseJigglerInterval);

			rdpFile.SetValue(KnownRdpFormatKeys.UseMultimon, rdpEndPoint.SelectedMonitorsFromId.HasValue ? 1 : 0);
			rdpFile.SetValue(
				KnownRdpFormatKeys.SelectedMonitors,
				EncodeSelectedMonitors(rdpEndPoint.SelectedMonitorsFromId, rdpEndPoint.SelectedMonitorsSpanCount)
			);


			var destPath = BuildDataConnectionPath(endPoint);

			rdpFile.SaveAs(destPath);

		}

		private (int? SelectedMonitorsFromId, int? SelectedMonitorsSpanCount) DecodeSelectedMonitors(string? rdpFileSelectedMonitorsValue) {

			if (string.IsNullOrEmpty(rdpFileSelectedMonitorsValue)) {
				return (null, null);
			}

			int[] selectedMonitors = rdpFileSelectedMonitorsValue
				.Split(',')
				.Select(x => {
					if (int.TryParse(x, out int i)) {
						return i;
					} else {
						return (int?)null;
					}
				})
				.OfType<int>()
				.Order()
				.ToArray()
				?? []
			;

			if (selectedMonitors.Length > 0) {
				int firstMonitor = selectedMonitors[0];
				int lastMonitor = selectedMonitors[^1];
				return (
					SelectedMonitorsFromId: firstMonitor,
					SelectedMonitorsSpanCount: lastMonitor - firstMonitor + 1
				);
			} else {
				return (null, null);
			}
		}

		private string EncodeSelectedMonitors(int? selectedMonitorsFromId, int? selectedMonitorsSpanCount) {
			if (selectedMonitorsFromId == null || selectedMonitorsSpanCount == null) {
				return "";
			}
			return string.Join(",", Enumerable.Range(selectedMonitorsFromId.Value, selectedMonitorsSpanCount.Value));
		}

		public RdpFileConnectionEndPoint Load(RdpFormatFile rdpFile) {
			var id = Path.GetFileNameWithoutExtension(rdpFile.Path)
				?? throw new Exception("Invalid RDP file path")
			;

			// decode selected monitors
			var (selectedMonitorsFromId, selectedMonitorsSpanCount) = DecodeSelectedMonitors(rdpFile.GetStringValue(KnownRdpFormatKeys.SelectedMonitors));


			return new RdpFileConnectionEndPoint(
				Id: id,
				Name: rdpFile.GetStringValue(KnownRdpFormatKeys.MeldName) ?? "",
				FullAddress: rdpFile.GetStringValue(KnownRdpFormatKeys.FullAddress) ?? "",
				RdpFilepath: rdpFile.Path,
				Group: rdpFile.GetStringValue(KnownRdpFormatKeys.MeldGroup) ?? "",

				EnableMouseJiggler: rdpFile.GetIntValue(KnownRdpFormatKeys.EnableMouseJiggler) == 1,
				MouseJigglerInterval: rdpFile.GetIntValue(KnownRdpFormatKeys.MouseJigglerInterval),

				SelectedMonitorsFromId: selectedMonitorsFromId,
				SelectedMonitorsSpanCount: selectedMonitorsSpanCount
			);
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
				var rdpEp = Load(rdpFile);
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
