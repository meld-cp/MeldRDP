namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	public class NullRdpMstscService : IRdpMstscService {
		public Process Connect(string path, Action? OnExitAction) {
			throw new NotImplementedException("NullRdpMstscService does not support Connect.");
		}

		public void EditRdpFile(string path, Action? OnExitAction) {

		}
	}
}
