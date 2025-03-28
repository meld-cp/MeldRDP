namespace MeldRDP.ViewModels {
	using MeldRDP.Models;
	using MeldRDP.Services;

	public class DesignEndPointListItemViewModel : EndPointListItemViewModel {
		public DesignEndPointListItemViewModel() : base(
			router: new NullRouter(),
			endPoint: new DesignConnectionEndPoint(),
			extendedInfo: "Extended Info",
			OnEditingCompleteAction: null
		) {
			this.Name = "Design End Point";
		}
	}

	public class DesignEndPointGroupViewModel : EndPointGroupViewModel {
		public DesignEndPointGroupViewModel() : base("Design End Point Group", false) { }

	}

}
