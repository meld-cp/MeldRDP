namespace MeldRDP.DesignTime {
	using MeldRDP.ViewModels;

	public class DesignConnectionEditorWindowViewModel : ConnectionEditorWindowViewModel {
		public DesignConnectionEditorWindowViewModel() : base(
			connRepo: new NullConnectionRepository(),
			endpoint: new DesignConnectionEndPoint(),
			backgroundImageProvider: new NullImageProvider()
		) { }
	}
}
