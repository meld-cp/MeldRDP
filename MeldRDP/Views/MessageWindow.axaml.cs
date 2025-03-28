namespace MeldRDP;

using Avalonia.ReactiveUI;

using MeldRDP.ViewModels;

public partial class MessageWindow : ReactiveWindow<MessageWindowViewModel> {
	public MessageWindow() {
		this.InitializeComponent();
	}
}