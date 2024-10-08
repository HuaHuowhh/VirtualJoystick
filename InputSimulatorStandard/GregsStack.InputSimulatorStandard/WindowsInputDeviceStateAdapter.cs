using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	public class WindowsInputDeviceStateAdapter : IInputDeviceStateAdapter
	{
		public bool IsKeyDown(VirtualKeyCode keyCode)
		{
			return NativeMethods.GetKeyState((ushort)keyCode) < 0;
		}

		public bool IsKeyUp(VirtualKeyCode keyCode)
		{
			return !IsKeyDown(keyCode);
		}

		public bool IsHardwareKeyDown(VirtualKeyCode keyCode)
		{
			return NativeMethods.GetAsyncKeyState((ushort)keyCode) < 0;
		}

		public bool IsHardwareKeyUp(VirtualKeyCode keyCode)
		{
			return !IsHardwareKeyDown(keyCode);
		}

		public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
		{
			return (NativeMethods.GetKeyState((ushort)keyCode) & 1) == 1;
		}
	}
}
