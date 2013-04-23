using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Eve.API.Touch {
	/// <summary>
	/// Touch provider contains methods for controlling clients cursor
	/// </summary>
	public static class TouchProvider {
		/// <summary>
		/// Starts provider and initializes components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Start() {
			TouchProvider.IsRuning = true;
		}

		/// <summary>
		/// Stops provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Stop() {
			TouchProvider.IsRuning = false;
		}

		/// <summary>
		/// Moves mouse position relative to its current position
		/// </summary>
		/// <param name="x">X move amount</param>
		/// <param name="y">Y move amount</param>
		public static void MoveMouse(int x, int y) {
			TouchProvider.SetMousePosition(
				System.Windows.Forms.Cursor.Position.X + x,
				System.Windows.Forms.Cursor.Position.Y + y);
		}

		/// <summary>
		/// Interpolates between start position and end position with acceleration
		/// </summary>
		/// <param name="x">End position X</param>
		/// <param name="y">End position Y</param>
		public static void MoveMouseSmart(double x, double y) {
			int steps = Math.Max(8, (int) Math.Ceiling((Math.Abs(x) + Math.Abs(y)) / 10.0));

			if (Math.Abs(x) > 20) x *= Math.Abs((x / 10) % 10 / 2);
			if (Math.Abs(y) > 20) y *= Math.Abs((y / 10) % 10 / 2);

			// Divide slope by number of steps
			x /= steps;
			y /= steps;

			// Interpolation variables
			double interX = System.Windows.Forms.Cursor.Position.X;
			double interY = System.Windows.Forms.Cursor.Position.Y;

			// Make small steps
			for (int index = 0; index < steps; index++) {
				interX += x;
				interY += y;
				TouchProvider.SetMousePosition((int) interX, (int) interY);
				System.Threading.Thread.Sleep(1);
			}

			// Set cursor to end position
			TouchProvider.SetMousePosition(
				(int) (System.Windows.Forms.Cursor.Position.X + x),
				(int) (System.Windows.Forms.Cursor.Position.Y + y));
		}

		/// <summary>
		/// Sets current cursor position to given coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		public static void SetMousePosition(int x, int y) {
			TouchProvider.Unmanaged.SetCursorPos(x, y);
		}

		/// <summary>
		/// Scrolls given amount by any step
		/// </summary>
		/// <param name="amount">Amount to scroll</param>
		/// <param name="step">Step to scroll (1 for down, -1 for up)</param>
		public static void ScrollMouse(uint amount, uint step = 1) {
			for (uint index = 0; index < amount; index += step) {
				TouchProvider.Unmanaged.mouse_event((uint)TouchProvider.Unmanaged.MouseEventFlags.WHEEL, 0, 0, step, 0);
			}
		}

		/// <summary>
		/// Press down and then up for given button
		/// </summary>
		/// <param name="button">Button to click</param>
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

		/// <summary>
		/// Gets indication whether provider is running
		/// </summary>
		public static bool IsRuning { get; private set; }

		#endregion

		/// <summary>
		/// Mapping enum for mouse buttons
		/// Can be directly mapped to Eve.API.Service buttons enum
		/// </summary>
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
