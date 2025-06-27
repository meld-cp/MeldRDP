namespace MeldRDP.Services {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;

	using MeldRDP.Models;

	public class FolderRepository : BasePathService, IConnectionRepository {

		public event EventHandler? ConnectionsChanged;

		public FolderRepository(string basePath) : base(basePath) {

		}

		private string BuildDataConnectionPath(string id) {
			return this.BuildFilePath($"{id}.rdp");
		}

		private string BuildDataConnectionPath(IConnectionEndPoint endPoint) {
			if (endPoint is RdpFileConnectionEndPoint rdpEndPoint) {
				return this.BuildDataConnectionPath(rdpEndPoint.Id);
			}
			throw new NotImplementedException();
		}

		public IConnectionEndPoint Create(string name, ConnectionGroup? group) {
			this.EnsureDirectoryExists();

			var id = ConnectionFactory.BuildNewId();

			var con = ConnectionFactory.BuildRdp(
				id: id,
				name: name,
				group: group,
				rdpFilePath: this.BuildDataConnectionPath(id)
			);

			this.Save(con);

			return con;
		}

		public void Save(IConnectionEndPoint endPoint) {
			this.EnsureDirectoryExists();

			if (endPoint is not RdpFileConnectionEndPoint rdpEndPoint) {
				throw new NotImplementedException();
			}

			var rdpFile = new RdpFormatFile(rdpEndPoint.RdpFilepath);
			rdpFile.SetValue(KnownRdpFormatKeys.FullAddress, rdpEndPoint.FullAddress);

			rdpFile.SetValue(KnownRdpFormatKeys.Meld.Name, rdpEndPoint.Name);
			rdpFile.SetValue(KnownRdpFormatKeys.Meld.Group, rdpEndPoint.Group);
			rdpFile.SetValue(KnownRdpFormatKeys.Meld.BackgroundImageName, rdpEndPoint.BackgroundImageName);
			rdpFile.SetValue(KnownRdpFormatKeys.Meld.IsPinned, rdpEndPoint.IsPinned ? 1 : 0);

			rdpFile.SetValue(KnownRdpFormatKeys.Ex.EnableMouseJiggler, rdpEndPoint.EnableMouseJiggler ? 1 : 0);
			rdpFile.SetValue(KnownRdpFormatKeys.Ex.AllowBackgroundInput, rdpEndPoint.EnableMouseJiggler ? 1 : 0);
			rdpFile.SetValue(KnownRdpFormatKeys.Ex.MouseJigglerInterval, rdpEndPoint.MouseJigglerInterval);

			rdpFile.SetValue(KnownRdpFormatKeys.UseMultimon, rdpEndPoint.SelectedMonitorsFromId.HasValue ? 1 : 0);
			rdpFile.SetValue(
				KnownRdpFormatKeys.SelectedMonitors,
				this.EncodeSelectedMonitors(rdpEndPoint.SelectedMonitorsFromId, rdpEndPoint.SelectedMonitorsSpanCount)
			);


			var destPath = this.BuildDataConnectionPath(endPoint);

			rdpFile.SaveAs(destPath);

			this.ConnectionsChanged?.Invoke(this, EventArgs.Empty);
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
			var (selectedMonitorsFromId, selectedMonitorsSpanCount) = this.DecodeSelectedMonitors(rdpFile.GetStringValue(KnownRdpFormatKeys.SelectedMonitors));


			return new RdpFileConnectionEndPoint(
				Id: id,
				Name: rdpFile.GetStringValue(KnownRdpFormatKeys.Meld.Name) ?? "",
				FullAddress: rdpFile.GetStringValue(KnownRdpFormatKeys.FullAddress) ?? "",
				RdpFilepath: rdpFile.Path,
				Group: rdpFile.GetStringValue(KnownRdpFormatKeys.Meld.Group) ?? "",
				BackgroundImageName: rdpFile.GetStringValue(KnownRdpFormatKeys.Meld.BackgroundImageName),
				IsPinned: rdpFile.GetIntValue(KnownRdpFormatKeys.Meld.IsPinned) == 1,

				EnableMouseJiggler: rdpFile.GetIntValue(KnownRdpFormatKeys.Ex.EnableMouseJiggler) == 1,
				MouseJigglerInterval: rdpFile.GetIntValue(KnownRdpFormatKeys.Ex.MouseJigglerInterval),

				SelectedMonitorsFromId: selectedMonitorsFromId,
				SelectedMonitorsSpanCount: selectedMonitorsSpanCount
			);
		}

		public IConnectionEndPoint[] FetchAll() {
			if (!this.BasePathExists()) {
				return [];
			}

			var result = new List<IConnectionEndPoint>();

			// fetch rdp files
			var rdpFiles = this
				.EnumerateFiles(
					searchPattern: "*.rdp",
					searchOption: SearchOption.TopDirectoryOnly
				)
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
				var rdpEp = this.Load(rdpFile);
				result.Add(rdpEp);
			}

			return [.. result];
		}

		public IConnectionEndPoint[] FetchByGroup(string groupName, IConnectionEndPoint[]? connections = null) {
			connections ??= this.FetchAll();
			return [.. connections.Where(ep => ep.Group == groupName)];
		}

		public void Remove(IConnectionEndPoint endPoint) {
			var destPath = this.BuildDataConnectionPath(endPoint);
			if (File.Exists(destPath)) {
				File.Delete(destPath);
			}
			this.ConnectionsChanged?.Invoke(this, EventArgs.Empty);
		}

		public ConnectionGroup[] FetchAllGroups() {
			return [.. this.FetchAll()
				.Select(ep => ep.Group)
				.Distinct()
				.Where(groupName => !string.IsNullOrWhiteSpace(groupName))
				.Select(groupName => new ConnectionGroup(ConnectionGroupType.Custom, groupName))
			];
		}

		public void NotifyConnectionsChanged() {
			this.ConnectionsChanged?.Invoke(this, EventArgs.Empty);
		}
	}

}
