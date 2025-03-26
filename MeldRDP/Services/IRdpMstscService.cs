namespace MeldRDP.Services {
	using System;
	using System.Diagnostics;

	public interface IRdpMstscService {
		Process Connect(string path, Action? OnExitAction);
		void EditRdpFile(string path, Action? OnExitAction);
	}

}
