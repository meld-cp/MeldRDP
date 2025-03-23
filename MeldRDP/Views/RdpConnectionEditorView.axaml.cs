namespace MeldRDP.Views;
using Avalonia.ReactiveUI;

using MeldRDP.ViewModels;

public partial class RdpConnectionEditorView : ReactiveUserControl<RdpConnectionEditorViewModel> {
	public RdpConnectionEditorView() {
		InitializeComponent();
	}
}