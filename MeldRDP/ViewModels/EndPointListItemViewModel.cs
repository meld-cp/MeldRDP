namespace MeldRDP.ViewModels {
	using System;
	using System.Windows.Input;

	using MeldRDP.Models;
	using MeldRDP.Services;

	using ReactiveUI;
	using ReactiveUI.Fody.Helpers;

	public class EndPointListItemViewModel : ViewModelBase {
		private readonly IRouter router;
		private readonly IConnectionEndPoint endPoint;
		private readonly Action? onEditingCompleteAction;

		[Reactive]
		public string Id { get; private set; }

		[Reactive]
		public string Name { get; set; }

		[Reactive]
		public string Group { get; set; }

		[Reactive]
		public string ExtendedInfo { get; set; }


		public ICommand ConnectCommand { get; }
		public ICommand EditCommand { get; }

		public string ExtendedEditLabel { get; } = "Edit (ex.)";
		public ICommand ExtendedEditCommand { get; }

		public EndPointListItemViewModel(
			IRouter router,
			IConnectionEndPoint endPoint,
			string extendedInfo,
			Action? OnEditingCompleteAction
		) {
			this.router = router;
			this.endPoint = endPoint;
			onEditingCompleteAction = OnEditingCompleteAction;

			this.Id = endPoint.Id;
			this.Name = endPoint.Name;
			this.ExtendedInfo = extendedInfo;
			this.Group = endPoint.Group;

			ConnectCommand = ReactiveCommand.Create(Connect);
			EditCommand = ReactiveCommand.Create(() => Edit(false));
			ExtendedEditCommand = ReactiveCommand.Create(() => Edit(true));
		}

		private void Connect() {
			router.Connect(endPoint);
		}

		private void Edit(bool extended) {
			router.Edit(
				endPoint: endPoint,
				extendedEdit: extended,
				OnEditingCompleteAction: onEditingCompleteAction
			);
		}

	}
}
