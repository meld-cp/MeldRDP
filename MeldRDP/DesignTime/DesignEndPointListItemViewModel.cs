namespace MeldRDP.DesignTime {
	using System;

	using MeldRDP.Models;
	using MeldRDP.ViewModels;

	public class DesignEndPointListItemViewModel : EndPointListItemViewModel {
		public DesignEndPointListItemViewModel() : base(
			router: new NullRouter(),
			endPoint: new DesignConnectionEndPoint(),
			extendedInfo: "Extended Info",
			extendedEdits: [new("Extended", ""), new("Text Editor", "")],
			backgroundImage: null,
			isPinned: Random.Shared.Next(0,1)==1
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
