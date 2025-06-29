﻿namespace MeldRDP.DesignTime {
	using System.Linq;

	using DynamicData;

	using MeldRDP.ViewModels;


	public class DesignMainViewModel : MainViewModel {

		public DesignMainViewModel() : base(
			router: new NullRouter(),
			connectionRepo: new DesignConnectionRepository(),
			backgroundProvider: new NullImageProvider(),
			defaultConnectionEndPointActionHandler: null,
			rdpFileConnectionEndPointActionHandler: null
		) {

			this.EndPointGroups.AddRange(
				Enumerable.Range(1, 10).Select(num => new DesignEndPointGroupViewModel())
			);

			this.ConnectionEndPoints.AddRange(
				Enumerable.Range(1, 10).Select(num => new DesignEndPointListItemViewModel())
			);

			this.SelectedGroupTitle = "Connection Group Title";
			this.ConnectionSummary = "Connections: 10";
		}
	}
}
