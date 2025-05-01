namespace MeldRDP.Models {
	public class ExtendedEditModel(string label, string editType) {
		public string Label { get; } = label;
		public string EditType { get; } = editType;
	}
}