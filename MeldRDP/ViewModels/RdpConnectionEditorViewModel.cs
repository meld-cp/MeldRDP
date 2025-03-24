namespace MeldRDP.ViewModels {
	using System;

	using MeldRDP.Models;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class RdpConnectionEditorViewModel : ViewModelBase {

		[Reactive]
		public bool EnableMouseJiggler { get; set; }

		[Reactive]
		public int? MouseJigglerInterval { get; set; }


		public RdpConnectionEditorViewModel(RdpFileConnectionEndPoint endpoint) {
			this.EnableMouseJiggler = endpoint.EnableMouseJiggler;
			this.MouseJigglerInterval = endpoint.MouseJigglerInterval;

			this.WhenAnyValue(x => x.EnableMouseJiggler)
				.Subscribe(enableJiggler => {
					if (enableJiggler && this.MouseJigglerInterval == null) {
						this.MouseJigglerInterval = 60;
					} else if (!enableJiggler && this.MouseJigglerInterval != null) {
						this.MouseJigglerInterval = null;
					}
				})
			;

			this.WhenAnyValue(x => x.MouseJigglerInterval)
				.Subscribe(interval => {
					if (interval == null && this.EnableMouseJiggler) {
						this.EnableMouseJiggler = false;
					} else if (interval != null && !this.EnableMouseJiggler) {
						this.EnableMouseJiggler = true;
					}
				})
			;
		}

	}

}
