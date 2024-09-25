using System;
using System.Runtime.InteropServices;

namespace GregsStack.InputSimulatorStandard.Native
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll", SetLastError = true)]
		public static extern short GetAsyncKeyState(ushort virtualKeyCode);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern short GetKeyState(ushort virtualKeyCode);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint SendInput(uint numberOfInputs, Input[] inputs, int sizeOfInputStructure);

		[DllImport("user32.dll")]
		public static extern IntPtr GetMessageExtraInfo();

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetCursorPos(out Point lpPoint);

		[DllImport("user32.dll")]
		public static extern uint GetSystemMetrics(uint smIndex);
	}
}
