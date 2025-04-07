namespace MeldRDP.DesignTime {
	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesignEndPointListItemViewModel : EndPointListItemViewModel {
		public DesignEndPointListItemViewModel() : base(
			router: new NullRouter(),
			endPoint: new DesignConnectionEndPoint(),
			extendedInfo: "Extended Info",
			backgroundImage: null,
			OnEditingCompleteAction: null
		) {
			this.Name = "Design End Point";
		}
	}

	public class DesignEndPointGroupViewModel : EndPointGroupViewModel {
		public DesignEndPointGroupViewModel() : base(ConnectionGroupType.Custom, "Design End Point Group") { }

	}

}
