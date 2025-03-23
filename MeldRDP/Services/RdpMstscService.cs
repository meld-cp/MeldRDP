namespace MeldRDP.Services {
	using System.Diagnostics;

	/// <summary>
	/// Uses the standard mstsc.exe to open RDP files
	/// </summary>
	public class RdpMstscService : IRdpMstscService {
		//"C:\WINDOWS\system32\mstsc.exe"
		//https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/mstsc

		public void EditRdpFile(string path) {
			Process.Start("mstsc.exe", ["/edit", string.Concat('"', path, '"')]);
		}

		public Process Connect(string path) {
			return Process.Start("mstsc.exe", [string.Concat('"', path, '"')]);
		}

	}


}
