namespace MeldRDP.Models {
	public static class KnownRdpFormatKeys {

		#region Standard Keys
		// See: https://learn.microsoft.com/en-us/azure/virtual-desktop/rdp-properties

		/// <summary>
		/// Specifies the hostname or IP address of the remote computer that you want to connect to.
		/// This is the only mandatory property in a .rdp file
		/// </summary>
		public const string FullAddress = "full address";

		/// <summary>
		/// Determines whether the remote session will use one or multiple displays from the local device.
		/// </summary>
		public const string UseMultimon = "use multimon";

		/// <summary>
		/// Specifies which local displays to use in a remote session.
		/// The selected displays must be contiguous. Requires use multimon set to 1.
		/// </summary>
		public const string SelectedMonitors = "selectedmonitors";

		#endregion Standard Keys


		#region Devolutions Keys
		//See: https://github.com/Devolutions/MsRdpEx

		// EnableMouseJiggler:i:1 - enable the mouse jiggler (disabled by default)
		public const string EnableMouseJiggler = "EnableMouseJiggler";

		// MouseJigglerInterval:i:60 - set mouse jiggler interval in seconds (60 seconds by default)
		public const string MouseJigglerInterval = "MouseJigglerInterval";

		// MouseJigglerMethod:i:0 set mouse jiggler method (0 = mouse move (default), 1 = special key (F15))
		public const string MouseJigglerMethod = "MouseJigglerMethod";


		#endregion Devolutions Keys

		#region Meld Keys

		public const string MeldName = "MeldName";
		public const string MeldGroup = "MeldGroup";
		public const string MeldBackgroundImageName = "MeldBackgroundImageName";

		#endregion Meld Keys
	}
}