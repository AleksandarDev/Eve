using System;

namespace Eve.API.Services.Common.Modules {
	[Flags]
	public enum Orientation : uint {
		Horizontal = 0,
		Vertical = 1
	}
}