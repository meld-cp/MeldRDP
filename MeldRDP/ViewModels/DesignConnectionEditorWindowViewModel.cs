namespace MeldRDP.ViewModels {
	using MeldRDP.Models;
	using MeldRDP.Services;

	public class DesignConnectionEditorWindowViewModel : ConnectionEditorWindowViewModel {
		public DesignConnectionEditorWindowViewModel() : base(
			connRepo: new NullConnectionRepository(),
			endpoint: new DesignConnectionEndPoint(),
			backgroundImageProvider: new NullImageProvider()
		) { }
	}
}
