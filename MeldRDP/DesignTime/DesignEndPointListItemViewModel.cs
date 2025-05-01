namespace MeldRDP.DesignTime {
	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesignEndPointListItemViewModel : EndPointListItemViewModel {
		public DesignEndPointListItemViewModel() : base(
			router: new NullRouter(),
			endPoint: new DesignConnectionEndPoint(),
			extendedInfo: "Extended Info",
			extendedEdits: [new("Extended", ""), new("Text Editor", "")],
			backgroundImage: null,
			OnEditingCompleteAction: null
		) {
			this.Name = "Design End Point";
		}
	}

	public class DesignEndPointGroupViewModel : EndPointGroupViewModel {
		public DesignEndPointGroupViewModel() : base(
			groupType: ConnectionGroupType.Custom,
			name: "Design End Point Group"
		) { }

	}

}
