namespace MeldRDP.DesignTime {
	using MeldRDP.ViewModels;

	public class DesignConnectionEditorViewModel : ConnectionEditorViewModel {
		public DesignConnectionEditorViewModel() : base(new DesignConnectionEndPoint(), new NullImageProvider()) {
		}
	}
}
