namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// Uses Devolutions' mstscex.exe to open RDP files.
	/// This includes additional features like the ability to enable 'mousejiggle' to
	/// keep a session from timing out.
	/// </summary>
	public class RdpExMstscService : BasePathService, IRdpMstscService {
		//See: https://github.com/Devolutions/MsRdpEx
		//See: https://github.com/Devolutions/MsRdpEx/pull/82

		private readonly ProcessMonitorService procMon;
		private readonly string binPath;

		public RdpExMstscService(ProcessMonitorService procMon, string basePath) : base(basePath) {
			// get bin path
			this.binPath = Path.Combine(basePath, "mstscex.exe");
			if (!File.Exists(this.binPath)) {
				throw new Exception($"'{this.binPath}' not found");
			}

			// check dll exists
			var dllPath = Path.Combine(basePath, "MsRdpEx.dll");
			if (!File.Exists(dllPath)) {
				throw new Exception($"'{dllPath}' not found");
			}

			this.procMon = procMon;
		}

		public void CreateRdpFile(string path) {
			File.WriteAllText(path, "");
		}

		public void EditRdpFile(string path, Action? OnExitAction) {
			var proc = Process.Start(this.binPath, ["/edit", string.Concat('"', path, '"')]);
			if (OnExitAction != null) {
				this.procMon.MonitorOnExit(proc, OnExitAction);
			}
		}

		public Process Connect(string path, Action? OnExitAction) {
			var proc = Process.Start(this.binPath, [string.Concat('"', path, '"')]);
			if (OnExitAction != null) {
				this.procMon.MonitorOnExit(proc, OnExitAction);
			}
			return proc;
		}

	}

}
