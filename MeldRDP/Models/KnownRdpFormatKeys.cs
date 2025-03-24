namespace MeldRDP.Models {
	public static class KnownRdpFormatKeys {

		#region Devolutions

		//See: https://github.com/Devolutions/MsRdpEx

		// EnableMouseJiggler:i:1 - enable the mouse jiggler (disabled by default)
		public const string EnableMouseJiggler = "EnableMouseJiggler";
		// MouseJigglerInterval:i:60 - set mouse jiggler interval in seconds (60 seconds by default)
		public const string MouseJigglerInterval = "MouseJigglerInterval";
		// MouseJigglerMethod:i:0 set mouse jiggler method (0 = mouse move (default), 1 = special key (F15))
		public const string MouseJigglerMethod = "MouseJigglerMethod";

		#endregion Devolutions

		#region Meld
		public const string MeldName = "MeldName";
		public const string MeldGroup = "MeldGroup";
		#endregion Meld
	}
}