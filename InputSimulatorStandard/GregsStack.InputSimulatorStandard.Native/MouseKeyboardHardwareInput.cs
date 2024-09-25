using System.Runtime.InteropServices;

namespace GregsStack.InputSimulatorStandard.Native
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct MouseKeyboardHardwareInput
	{
		[FieldOffset(0)]
		public MouseInput Mouse;

		[FieldOffset(0)]
		public KeyboardInput Keyboard;

		[FieldOffset(0)]
		public HardwareInput Hardware;
	}
}
