using System;

namespace GregsStack.InputSimulatorStandard.Native
{
	internal struct KeyboardInput
	{
		public ushort KeyCode;

		public ushort Scan;

		public uint Flags;

		public uint Time;

		public IntPtr ExtraInfo;
	}
}
