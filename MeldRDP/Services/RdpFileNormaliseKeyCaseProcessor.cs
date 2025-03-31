namespace MeldRDP.Services {
	/// <summary>
	/// Keys are all lower-cased when saving from 'mstsc /edit', mstcex is
	/// not case sensitive so we need to normalise the keys again.
	/// </summary>
	public class RdpFileNormaliseKeyCaseProcessor(string[] KeysToNormalise) : IRdpFileProcessor {
		public void Process(string filepath) {
			var f = new RdpFormatFile(filepath);
			var changeCount = f.NormaliseKeyCase(KeysToNormalise);
			if (changeCount > 0) {
				f.Save();
			}
		}
	}
}
