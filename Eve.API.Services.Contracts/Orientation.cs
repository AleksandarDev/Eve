using System;

namespace Eve.API.Services.Contracts {
	[Flags]
	public enum Orientation : uint {
		Horizontal = 0,
		Vertical = 1
	}
}