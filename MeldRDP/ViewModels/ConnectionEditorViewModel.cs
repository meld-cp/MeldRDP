namespace MeldRDP.ViewModels {
	using System;

	using Avalonia.Media.Imaging;

	using MeldRDP.Models;
	using MeldRDP.Services;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class ConnectionEditorViewModel : ViewModelBase {

		[Reactive]
		public string Id { get; set; }

		[Reactive]
		public string Name { get; set; }

		[Reactive]
		public string Group { get; set; }

		[Reactive]
		public string? BackgroundImageName { get; set; }

		[Reactive]
		public Bitmap? BackgroundSource { get; set; }

		public ConnectionEditorViewModel(IConnectionEndPoint endpoint, IImageProvider backgroundImageProvider) {
			this.Id = endpoint.Id;
			this.Name = endpoint.Name;
			this.Group = endpoint.Group;
			this.BackgroundImageName = endpoint.BackgroundImageName;

			this.WhenAnyValue(x => x.BackgroundImageName)
				.Subscribe(x => this.BackgroundSource = backgroundImageProvider.Fetch(x))
			;
		}
	}

}
