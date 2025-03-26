namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	/// <summary>
	/// Uses the standard mstsc.exe to open RDP files
	/// </summary>
	public class RdpMstscService : IRdpMstscService {
		private readonly ProcessMonitorService procMon;

		//"C:\WINDOWS\system32\mstsc.exe"
		//https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/mstsc

		public RdpMstscService(ProcessMonitorService procMon) {
			this.procMon = procMon;
		}

		public void EditRdpFile(string path, Action? OnExitAction) {
			var proc = Process.Start("mstsc.exe", ["/edit", string.Concat('"', path, '"')]);
			if (OnExitAction != null) {
				this.procMon.MonitorOnExit(proc, OnExitAction);
			}
		}

		public Process Connect(string path, Action? OnExitAction) {
			var proc = Process.Start("mstsc.exe", [string.Concat('"', path, '"')]);
			if (OnExitAction != null) {
				this.procMon.MonitorOnExit(proc, OnExitAction);
			}
			return proc;
		}

	}
}
