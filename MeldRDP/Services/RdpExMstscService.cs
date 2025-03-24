namespace MeldRDP.Services {
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// Uses Devolutions' mstscex.exe to open RDP files.
	/// This includes additional features like the ability to enable 'mousejiggle' to
	/// keep a session from timing out.
	/// </summary>
	public class RdpExMstscService : IRdpMstscService {
		//See: https://github.com/Devolutions/MsRdpEx
		//See: https://github.com/Devolutions/MsRdpEx/pull/82

		private readonly string binPath;

		public RdpExMstscService(string basePath) {
			// get bin path
			this.binPath = Path.Combine(basePath, "mstscex.exe");
			if (!File.Exists(this.binPath)) {
				throw new System.Exception($"'{this.binPath}' not found");
			}

			// check dll exists
			var dllPath = Path.Combine(basePath, "MsRdpEx.dll");
			if (!File.Exists(dllPath)) {
				throw new System.Exception($"'{dllPath}' not found");
			}
		}

		public void CreateRdpFile(string path) {
			File.WriteAllText(path, "");
		}

		public void EditRdpFile(string path) {
			Process.Start(this.binPath, ["/edit", string.Concat('"', path, '"')]);
		}

		public Process Connect(string path) {
			return Process.Start(this.binPath, [string.Concat('"', path, '"')]);
		}

	}

}
