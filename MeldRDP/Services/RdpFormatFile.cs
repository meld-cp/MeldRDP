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

		// Public methods

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

				this.SetValue(key, type, value);
			}

		}

		public void Save() {
			this.SaveAs(this.Path);
		}

		public void SaveAs(string path) {
			File.WriteAllLines(path, this.Records.Select(x => $"{x.Key}:{x.Type}:{x.Value}"));
		}

		public RdpFileFormatRecord? GetRecord(string key) {
			return this.GetRecordWithIndex(key).Rec;
		}

		public string? GetStringValue(string key) {
			var rec = this.GetRecord(key);
			if (rec == null || rec.Type != TYPE_STRING) {
				return null;
			}
			return rec.Value;
		}

		public int? GetIntValue(string key) {
			var rec = this.GetRecord(key);
			if (rec == null || rec.Type != TYPE_INT) {
				return null;
			}
			var val = rec.Value;
			if (!int.TryParse(val, out var i)) {
				return null;
			}
			return i;
		}

		public void SetValue(string key, string? value) {
			this.SetValue(key, TYPE_STRING, value);
		}

		public void SetValue(string key, int? value) {
			this.SetValue(key, TYPE_INT, $"{value}");
		}

		/// <summary>
		/// Check keys with unexpected Key cases and fix them
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public int NormaliseKeyCase(string[] keys) {

			var count = 0;

			foreach (var normKey in keys) {

				var (recIdx, rec) = this.GetRecordWithIndex(normKey);
				if (!recIdx.HasValue) {
					continue; // key was not found, skip
				}

				// if we have an index, we should have a rec
				System.Diagnostics.Debug.Assert(rec != null);

				if (rec.Key.Equals(normKey, System.StringComparison.InvariantCulture)) {
					continue; // keys are same case, skip
				}

				// need to fix the key case
				this.Records[recIdx.Value] = rec with {
					Key = normKey
				};

				count++;
			}

			return count;

		}

		// Private methods
		private (int? Index, RdpFileFormatRecord? Rec) GetRecordWithIndex(string key) {
			var recIdx = this.Records.FindIndex(x => x.Key.Equals(key, System.StringComparison.InvariantCultureIgnoreCase));
			if (recIdx == -1) {
				return (null, null);
			} else {
				return (recIdx, this.Records[recIdx]);
			}
		}

		private void SetValue(string key, string type, string? value) {

			var newRec = new RdpFileFormatRecord(key, type, value);

			var (recIdx, _) = this.GetRecordWithIndex(key);
			if (recIdx.HasValue) {
				this.Records[recIdx.Value] = newRec;
			} else {
				this.Records.Add(newRec);
			}

		}

	}
}
