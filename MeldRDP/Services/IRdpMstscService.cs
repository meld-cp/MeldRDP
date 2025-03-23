namespace MeldRDP.Services {
	using System.Diagnostics;

	public interface IRdpMstscService {
		Process Connect(string path);
		void EditRdpFile(string path);
	}

}
