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
			var procStartInfo = new ProcessStartInfo {
				FileName = this.binPath,
				Arguments = string.Concat('"', path, '"'),
				UseShellExecute = false,
			};

			procStartInfo.EnvironmentVariables.Add("MSRDPEX_LOG_ENABLED", "1");

			/*
				Used => TRACE, DEBUG,
				Unused => INFO, WARN, ERROR, FATAL

				#define MSRDPEX_LOG_TRACE   0
				#define MSRDPEX_LOG_DEBUG   1

				#define MSRDPEX_LOG_INFO    2
				#define MSRDPEX_LOG_WARN    3
				#define MSRDPEX_LOG_ERROR   4
				#define MSRDPEX_LOG_FATAL   5
				#define MSRDPEX_LOG_OFF     6
			*/
			procStartInfo.EnvironmentVariables.Add("MSRDPEX_LOG_LEVEL", "1");
			procStartInfo.EnvironmentVariables.Add("MSRDPEX_LOG_FILE_PATH", path + ".log");

			var proc = Process.Start(procStartInfo)
				?? throw new Exception("Failed to start process")
			;
			if (OnExitAction != null) {
				this.procMon.MonitorOnExit(proc, OnExitAction);
			}
			return proc;
		}

	}

}
