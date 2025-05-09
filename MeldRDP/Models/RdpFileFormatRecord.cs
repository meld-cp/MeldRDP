﻿namespace MeldRDP.Models {
	public record RdpFileFormatRecord(string Key, string Type, string? Value) {
		public new bool Equals(object? x, object? y) {
			if (x is not RdpFileFormatRecord xRecord || y is not RdpFileFormatRecord yRecord) {
				return false;
			}
			return xRecord.Key.Equals(yRecord.Key, System.StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode() {
			return this.Key.GetHashCode();
		}

	};
}
