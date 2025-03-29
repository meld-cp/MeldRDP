namespace MeldRDP.ViewModels {
	using MeldRDP.Models;
	using MeldRDP.Services;

	public class DesignConnectionEditorViewModel : ConnectionEditorViewModel {
		public DesignConnectionEditorViewModel() : base(new DesignConnectionEndPoint(), new NullImageProvider()) {
		}
	}
}
