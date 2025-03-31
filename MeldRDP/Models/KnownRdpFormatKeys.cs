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


		public static class Ex {
			// Devolutions Extended Keys
			// See: https://github.com/Devolutions/MsRdpEx

			// EnableMouseJiggler:i:1 - enable the mouse jiggler (disabled by default)
			public const string EnableMouseJiggler = "EnableMouseJiggler";

			// MouseJigglerInterval:i:60 - set mouse jiggler interval in seconds (60 seconds by default)
			public const string MouseJigglerInterval = "MouseJigglerInterval";

			// MouseJigglerMethod:i:0 set mouse jiggler method (0 = mouse move (default), 1 = special key (F15))
			public const string MouseJigglerMethod = "MouseJigglerMethod";

			// AllowBackgroundInput:i:1 Specifies whether background input mode is enabled.
			// When background input is enabled the client can accept input when the client
			// does not have focus.
			public const string AllowBackgroundInput = "AllowBackgroundInput";


			public static readonly string[] All = [
				EnableMouseJiggler,
				MouseJigglerInterval,
				MouseJigglerMethod,
				AllowBackgroundInput
			];
		}

		public static class Meld {
			public const string Name = "MeldName";
			public const string Group = "MeldGroup";
			public const string BackgroundImageName = "MeldBackgroundImageName";

			public static readonly string[] All = [
				Name,
				Group,
				BackgroundImageName
			];
		}
	}
}