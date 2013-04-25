﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Eve.Diagnostics.Logging;

namespace Eve.API.Touch {
	/// <summary>
	/// This provider contains methods for controlling clients cursor
	/// </summary>
	public static class TouchProvider {
		private static readonly Log.LogInstance log =
			new Log.LogInstance(typeof(TouchProvider));

		/// <summary>
		/// Starts provider and initializes components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Start() {
			// Make sure we don't start provider twice
			if (TouchProvider.IsRunning) {
				TouchProvider.log.Warn("Provider is already started");
				return;
			}

			TouchProvider.log.Info("Starting provider...");

			TouchProvider.IsRunning = true;
			TouchProvider.log.Info("Provider started");
		}

		/// <summary>
		/// Stops provider and its components
		/// </summary>
		/// <returns>Returns asynchronous void Task</returns>
		public async static Task Stop() {
			TouchProvider.log.Info("Stopping provider...");

			TouchProvider.IsRunning = false;
			TouchProvider.log.Info("Provider stopped");
		}

		/// <summary>
		/// Moves mouse position relative to its current position
		/// </summary>
		/// <param name="x">X move amount</param>
		/// <param name="y">Y move amount</param>
		/// <param name="overrideEvents">Overrides mouse move events</param>
		public static void MoveMouse(int x, int y, bool overrideEvents = false) {
			if (!TouchProvider.CheckIsRunning())
				return;

			if (overrideEvents)
				TouchProvider.SetMousePosition(
					System.Windows.Forms.Cursor.Position.X + x,
					System.Windows.Forms.Cursor.Position.Y + y);
			else
				TouchProvider.Unmanaged.mouse_event(
					(uint) TouchProvider.Unmanaged.MouseEventFlags.MOVE,
					(uint) x, (uint) y, 0, 0);
		}

		/// <summary>
		/// Interpolates between start position and end position with acceleration
		/// </summary>
		/// <param name="x">End position X</param>
		/// <param name="y">End position Y</param>
		public static void MoveMouseSmart(double x, double y) {
			if (!TouchProvider.CheckIsRunning())
				return;

			int steps = Math.Max(2, (int)Math.Ceiling((Math.Abs(x) + Math.Abs(y)) / 10.0));

			// Accelerate movement
			// TODO Remove, not used
			//if (Math.Abs(x) > 20) x *= Math.Abs((x / 10) % 10 / 2);
			//if (Math.Abs(y) > 20) y *= Math.Abs((y / 10) % 10 / 2);

			// Divide slope by number of steps
			x /= steps;
			y /= steps;

			// Make small steps
			for (int index = 0; index < steps; index++) {
				TouchProvider.MoveMouse((int)x, (int)y);
				System.Threading.Thread.Sleep(2);
			}
		}

		/// <summary>
		/// Sets current cursor position to given coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <param name="overrideEvents">
		/// Overrides mouse move events. 
		/// When set to false, only works on one display client configurations
		/// </param>
		public static void SetMousePosition(int x, int y, bool overrideEvents = true) {
			if (!TouchProvider.CheckIsRunning())
				return;

			if (overrideEvents) TouchProvider.Unmanaged.SetCursorPos(x, y);
			else
				TouchProvider.Unmanaged.mouse_event(
					(uint) TouchProvider.Unmanaged.MouseEventFlags.ABSOLUTE, (uint)x, (uint)y, 0, 0);
		}

		/// <summary>
		/// Scrolls given amount by any step
		/// </summary>
		/// <param name="amount">Amount to scroll</param>
		/// <param name="step">Step to scroll (1 for down, -1 for up)</param>
		public static void ScrollMouse(uint amount, uint step = 1) {
			if (!TouchProvider.CheckIsRunning())
				return;

			for (uint index = 0; index < amount; index += step) {
				TouchProvider.Unmanaged.mouse_event((uint)TouchProvider.Unmanaged.MouseEventFlags.WHEEL, 0, 0, step, 0);
			}
		}

		/// <summary>
		/// Press down and then up for given button
		/// </summary>
		/// <param name="button">Button to click</param>
		public static void ClickButton(Buttons button) {
			if (!TouchProvider.CheckIsRunning())
				return;

			switch (button) {
				default:
				case Buttons.Left:
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.LEFTDOWN);
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.LEFTUP);
					break;
				case Buttons.Middle:
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.MIDDLEDOWN);
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.MIDDLEUP);
					break;
				case Buttons.Right:
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.RIGHTDOWN);
					TouchProvider.DoButton(Unmanaged.MouseEventFlags.RIGHTUP);
					break;
			}
		}

		/// <summary>
		/// Calls mouse button event with given flag
		/// </summary>
		/// <param name="flag">Flag of button event to execute</param>
		/// <exception cref="InvalidOperationException">Throws exception on invalid flag, only button flags are allowed</exception>
		private static void DoButton(TouchProvider.Unmanaged.MouseEventFlags flag) {
			if (!TouchProvider.CheckIsRunning())
				return;

			if (flag.HasFlag(TouchProvider.Unmanaged.MouseEventFlags.ABSOLUTE) ||
				flag.HasFlag(TouchProvider.Unmanaged.MouseEventFlags.MOVE) ||
				flag.HasFlag(TouchProvider.Unmanaged.MouseEventFlags.WHEEL))
				throw new InvalidOperationException("Given flag isn't for button! flag is invalid for this operation");

			TouchProvider.Unmanaged.mouse_event((uint)flag, 0, 0, 0, 0);
		}

		/// <summary>
		/// Check whether provider is running - writes warning if it's not running
		/// </summary>
		/// <returns>Returns Boolean value indicating whether provider is started</returns>
		private static bool CheckIsRunning() {
			if (!TouchProvider.IsRunning)
				TouchProvider.log.Warn("Start provider before using it!");

			return TouchProvider.IsRunning;
		}


		#region Properties

		/// <summary>
		/// Gets indication whether provider is running
		/// </summary>
		public static bool IsRunning { get; private set; }

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
			// NOTE http://www.pinvoke.net/default.aspx/user32/mouse_event.html
			// NOTE http://www.pinvoke.net/default.aspx/user32/SetCursorPos.html

			[DllImport("user32.Dll")]
			public static extern long SetCursorPos(int x, int y);

			[DllImport("user32.dll")]
			public static extern void mouse_event(uint dwFlags, uint dx, uint dy,
												  uint dwData, int dwExtraInfo);

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
