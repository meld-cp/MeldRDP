namespace MeldRDP.Services {
	using System;

	using MeldRDP.Models;

	public class RdpFileConnectionEndPointActionHandler(
		IRouter router,
		IRdpFileProcessor rdpFileProcesser,
		IRdpMstscService srvRdp,
		IConnectionRepository connRepo
	) : IConnectionEndPointActionHandler {

		public void Connect(IConnectionEndPoint endPoint) {
			if (endPoint is not RdpFileConnectionEndPoint epRdp) {
				throw new NotImplementedException();
			}

			rdpFileProcesser?.Process(epRdp.RdpFilepath);

			_ = srvRdp.Connect(epRdp.RdpFilepath, null);

			return;
		}

		public void Edit(
			string editType,
			IConnectionEndPoint endPoint,
			Action? onCompleteAction
		) {

			if (endPoint is not RdpFileConnectionEndPoint epRdp) {
				throw new NotImplementedException();
			}

			switch (editType) {
				case ConnectionEditTypes.InApp:
					router.OpenInAppEditor(endPoint);
					return;


				case ConnectionEditTypes.Extended:
					srvRdp.EditRdpFile(
						path: epRdp.RdpFilepath,
						OnExitAction: router.RunOnMainThread(() => {
							connRepo.NotifyConnectionsChanged();
							onCompleteAction?.Invoke();
						})
					);
					return;

				case ConnectionEditTypes.TextEditor:
					router.OpenTextEditor(
						filepath: epRdp.RdpFilepath,
						onEditingCompleteAction: router.RunOnMainThread(() => {
							connRepo.NotifyConnectionsChanged();
							onCompleteAction?.Invoke();
						})
					);
					return;

			}

			throw new NotImplementedException($"Edit type '{editType}' not implemented for '{endPoint.GetType().Name}'");
		}

		public void SetPinned(IConnectionEndPoint endPoint, bool isPinned) {
			if (endPoint is not RdpFileConnectionEndPoint epRdp) {
				throw new NotImplementedException();
			}

			connRepo.Save(
				epRdp with {
					IsPinned = isPinned
				}
			);
		}
	}
}