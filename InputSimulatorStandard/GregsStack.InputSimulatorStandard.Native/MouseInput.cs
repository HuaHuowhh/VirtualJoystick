using System;

namespace GregsStack.InputSimulatorStandard.Native
{
	internal struct MouseInput
	{
		public int X;

		public int Y;

		public uint MouseData;

		public uint Flags;

		public uint Time;

		public IntPtr ExtraInfo;
	}
}
