namespace MeldRDP.ViewModels {
	using MeldRDP.Models;
	using MeldRDP.Services;

	public class DesignEndPointListItemViewModel : EndPointListItemViewModel {
		public DesignEndPointListItemViewModel() : base(new NullRouter(), new DesignConnectionEndPoint()) {
			Name = "Design End Point";
		}
	}

	public class DesignEndPointGroupViewModel : EndPointGroupViewModel {
		public DesignEndPointGroupViewModel() : base("Design End Point Group", false) { }

	}

}
