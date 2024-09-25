using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	public interface IInputDeviceStateAdapter
	{
		bool IsKeyDown(VirtualKeyCode keyCode);

		bool IsKeyUp(VirtualKeyCode keyCode);

		bool IsHardwareKeyDown(VirtualKeyCode keyCode);

		bool IsHardwareKeyUp(VirtualKeyCode keyCode);

		bool IsTogglingKeyInEffect(VirtualKeyCode keyCode);
	}
}
