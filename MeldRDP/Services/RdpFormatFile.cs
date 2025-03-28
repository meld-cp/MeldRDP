namespace MeldRDP.Services {
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using MeldRDP.Models;

	public class RdpFormatFile {
		private const string TYPE_INT = "i";
		private const string TYPE_STRING = "s";

		public string Path { get; }

		public List<RdpFileFormatRecord> Records { get; } = [];

		public RdpFormatFile(string path) {
			this.Path = path;
			this.Load();
		}

		public void Load() {

			this.Records.Clear();

			if (!File.Exists(this.Path)) {
				return;
			}

			var lines = File.ReadAllLines(this.Path);

			for (int i = 0; i < lines.Length; i++) {

				var parts = lines[i].Split(':');

				if (parts.Length < 2) {
					continue;
				}

				var key = parts[0];
				var type = parts[1];
				var value = string.Join(":", parts.Skip(2));

				this.Records.Add(new RdpFileFormatRecord(key, type, value));
			}

		}

		public void Save() {
			this.SaveAs(this.Path);
		}

		public void SaveAs(string path) {
			File.WriteAllLines(path, this.Records.Select(x => $"{x.Key}:{x.Type}:{x.Value}"));
		}

		public string? GetStringValue(string key) {
			return this.Records.FirstOrDefault(x => x.Key == key && x.Type == TYPE_STRING)?.Value;
		}

		public int? GetIntValue(string key) {
			var val = this.Records.FirstOrDefault(x => x.Key == key && x.Type == TYPE_INT)?.Value;
			if (!int.TryParse(val, out var i)) {
				return null;
			}
			return i;
		}

		private void SetValue(string key, string type, string? value) {

			var newRec = new RdpFileFormatRecord(key, type, value);

			var recIdx = this.Records.FindIndex(x => x.Key == key);
			if (recIdx == -1) {
				this.Records.Add(newRec);
			} else {
				this.Records[recIdx] = newRec;
			}

		}

		public void SetValue(string key, string? value) {
			this.SetValue(key, TYPE_STRING, value);
		}

		public void SetValue(string key, int? value) {
			this.SetValue(key, TYPE_INT, $"{value}");
		}
	}
}
