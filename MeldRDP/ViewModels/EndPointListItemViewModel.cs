namespace MeldRDP.ViewModels {
	using System.Collections.ObjectModel;
	using System.Windows.Input;

	using Avalonia.Media.Imaging;

	using MeldRDP.Models;
	using MeldRDP.Services;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class EndPointListItemViewModel : ViewModelBase {
		private readonly IRouter router;
		private readonly IConnectionEndPoint endPoint;

		[Reactive]
		public string Id { get; private set; }

		[Reactive]
		public string Name { get; set; }

		[Reactive]
		public string Group { get; set; }

		[Reactive]
		public string ExtendedInfo { get; set; }

		[Reactive]
		public Bitmap? BackgroundSource { get; set; }

		[Reactive]
		public bool IsPinned { get; set; }


		public ICommand ConnectCommand { get; }
		public ICommand EditCommand { get; }
		public ICommand ExtendedEditCommand { get; }
		public ICommand TogglePinnedCommand { get; }


		[Reactive]
		public ObservableCollection<ExtendedEditModel> ExtendedEdits { get; private set; }

		public EndPointListItemViewModel(
			IRouter router,
			IConnectionEndPoint endPoint,
			string extendedInfo,
			ExtendedEditModel[] extendedEdits,
			Bitmap? backgroundImage,
			bool isPinned
		) {
			this.router = router;
			this.endPoint = endPoint;

			this.Id = endPoint.Id;
			this.Name = endPoint.Name;
			this.ExtendedInfo = extendedInfo;


			this.Group = endPoint.Group;
			this.BackgroundSource = backgroundImage;
			this.IsPinned = isPinned;

			this.ConnectCommand = ReactiveCommand.Create(this.Connect);
			this.EditCommand = ReactiveCommand.Create(() => this.Edit(editType: ConnectionEditTypes.InApp));
			this.ExtendedEditCommand = ReactiveCommand.Create<string>(this.Edit);
			this.TogglePinnedCommand = ReactiveCommand.Create(this.TogglePinned);

			this.ExtendedEdits = [.. extendedEdits];

		}

		private void TogglePinned() {
			this.IsPinned = !this.IsPinned;
			this.router.SetPinned( this.endPoint, this.IsPinned );
		}

		private void Connect() {
			this.router.Connect(this.endPoint);
		}

		private void Edit(string editType) {
			this.router.Edit(
				editType: editType,
				endPoint: this.endPoint,
				onCompleteAction: null
			);
		}

	}
}
