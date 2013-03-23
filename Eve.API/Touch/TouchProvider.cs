using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Eve.API.Touch {
	public static class TouchProvider {
		public async static Task Start() {
			TouchProvider.IsRuning = true;
		}

		public async static Task Stop() {
			TouchProvider.IsRuning = false;
		}


		public static void MoveMouse(int x, int y) {
			TouchProvider.SetMousePosition(
				System.Windows.Forms.Cursor.Position.X + x,
				System.Windows.Forms.Cursor.Position.Y + y);
		}

		public static void SetMousePosition(int x, int y) {
			TouchProvider.Unmanaged.SetCursorPos(x, y);
		}

		public static void ScrollMouse(uint amount, uint step = 1) {
			for (uint index = 0; index < amount; index += step) {
				TouchProvider.Unmanaged.mouse_event((uint)TouchProvider.Unmanaged.MouseEventFlags.WHEEL, 0, 0, step, 0);
			}
		}

		public static void ClickButton(Buttons button) {
			switch (button) {
				default:
				case Buttons.Left:
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.LEFTUP, 0, 0, 0, 0);
					break;
				case Buttons.Middle:
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.MIDDLEDOWN, 0, 0, 0, 0);
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.MIDDLEUP, 0, 0, 0, 0);
					break;
				case Buttons.Right:
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.RIGHTDOWN, 0, 0, 0, 0);
					TouchProvider.Unmanaged.mouse_event((uint) TouchProvider.Unmanaged.MouseEventFlags.RIGHTUP, 0, 0, 0, 0);
					break;
			}
		}


		#region Properties

		public static bool IsRuning { get; private set; }

		#endregion


		public enum Buttons : uint {
			Left = 1,
			Right = 2,
			Middle = 3
		}

		private static class Unmanaged {
			[DllImport("User32.Dll")]
			public static extern long SetCursorPos(int x, int y);

			[DllImport("user32.dll")]
			public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData,
			                                       int dwExtraInfo);

			[Flags]
			public enum MouseEventFlags : uint {
				// ReSharper disable InconsistentNaming
				LEFTDOWN = 0x00000002,
				LEFTUP = 0x00000004,
				MIDDLEDOWN = 0x00000020,
				MIDDLEUP = 0x00000040,
				MOVE = 0x00000001,
				ABSOLUTE = 0x00008000,
				RIGHTDOWN = 0x00000008,
				RIGHTUP = 0x00000010,
				WHEEL = 0x00000800,
				XDOWN = 0x00000080,
				XUP = 0x00000100
				// ReSharper restore InconsistentNaming
			}

			//Use the values of this enum for the 'dwData' parameter
			//to specify an X button when using MouseEventFlags.XDOWN or
			//MouseEventFlags.XUP for the dwFlags parameter.
			public enum MouseEventDataXButtons : uint {
				// ReSharper disable InconsistentNaming
				XBUTTON1 = 0x00000001,
				XBUTTON2 = 0x00000002
				// ReSharper restore InconsistentNaming
			}
		}
	}
}
