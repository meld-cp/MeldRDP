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

		[Reactive]
		public int? SelectedMonitorsFromId { get; set; }

		[Reactive]
		public int? SelectedMonitorsSpanCount { get; set; }



		public RdpConnectionEditorViewModel(RdpFileConnectionEndPoint endpoint) {
			this.EnableMouseJiggler = endpoint.EnableMouseJiggler;
			this.MouseJigglerInterval = endpoint.MouseJigglerInterval;
			this.SelectedMonitorsFromId = endpoint.SelectedMonitorsFromId;
			this.SelectedMonitorsSpanCount = endpoint.SelectedMonitorsSpanCount;

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

			this.WhenAnyValue(x => x.SelectedMonitorsFromId)
				.Subscribe(fromId => {
					if (fromId != null && !this.SelectedMonitorsSpanCount.HasValue) {
						this.SelectedMonitorsSpanCount = 1;
					}
				})
			;
			this.WhenAnyValue(x => x.SelectedMonitorsSpanCount)
				.Subscribe(span => {
					if (span != null && !this.SelectedMonitorsFromId.HasValue) {
						this.SelectedMonitorsFromId = 0;
					}
				})
			;
		}

	}

}
