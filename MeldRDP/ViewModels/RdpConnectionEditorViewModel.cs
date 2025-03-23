namespace MeldRDP.ViewModels {
	using System;

	using MeldRDP.Models;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class RdpConnectionEditorViewModel : ViewModelBase {

		[Reactive]
		public bool KeepSessionAlive { get; set; }

		[Reactive]
		public int? MouseJigglerInterval { get; set; }


		public RdpConnectionEditorViewModel(RdpFileConnectionEndPoint endpoint) {
			this.KeepSessionAlive = endpoint.MouseJigglerInterval.HasValue;
			this.MouseJigglerInterval = endpoint.MouseJigglerInterval;

			this.WhenAnyValue(x => x.KeepSessionAlive)
				.Subscribe(keepAlive => {
					if (keepAlive && this.MouseJigglerInterval == null) {
						this.MouseJigglerInterval = 60;
					} else if (!keepAlive && this.MouseJigglerInterval != null) {
						this.MouseJigglerInterval = null;
					}
				})
			;

			this.WhenAnyValue(x => x.MouseJigglerInterval)
				.Subscribe(interval => {
					if (interval == null && this.KeepSessionAlive) {
						this.KeepSessionAlive = false;
					} else if (interval != null && !this.KeepSessionAlive) {
						this.KeepSessionAlive = true;
					}
				})
			;
		}

	}

}
