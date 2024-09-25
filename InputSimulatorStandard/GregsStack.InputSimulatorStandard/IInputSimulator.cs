namespace GregsStack.InputSimulatorStandard
{
	public interface IInputSimulator
	{
		IKeyboardSimulator Keyboard { get; }

		IMouseSimulator Mouse { get; }

		IInputDeviceStateAdapter InputDeviceState { get; }
	}
}
