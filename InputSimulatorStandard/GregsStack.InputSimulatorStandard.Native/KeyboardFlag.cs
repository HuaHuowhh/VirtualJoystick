using System;

namespace GregsStack.InputSimulatorStandard.Native
{
	[Flags]
	internal enum KeyboardFlag : uint
	{
		ExtendedKey = 1u,
		KeyUp = 2u,
		Unicode = 4u,
		ScanCode = 8u
	}
}
