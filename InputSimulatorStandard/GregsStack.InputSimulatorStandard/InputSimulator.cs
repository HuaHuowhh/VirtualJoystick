using System;

namespace GregsStack.InputSimulatorStandard
{
	public class InputSimulator : IInputSimulator
	{
		public IKeyboardSimulator Keyboard { get; }

		public IMouseSimulator Mouse { get; }

		public IInputDeviceStateAdapter InputDeviceState { get; }

		public InputSimulator()
			: this(new KeyboardSimulator(), new MouseSimulator(), new WindowsInputDeviceStateAdapter())
		{
		}

		public InputSimulator(IKeyboardSimulator keyboardSimulator, IMouseSimulator mouseSimulator, IInputDeviceStateAdapter inputDeviceStateAdapter)
		{
			Keyboard = keyboardSimulator ?? throw new ArgumentNullException("keyboardSimulator");
			Mouse = mouseSimulator ?? throw new ArgumentNullException("mouseSimulator");
			InputDeviceState = inputDeviceStateAdapter ?? throw new ArgumentNullException("inputDeviceStateAdapter");
		}
	}
}
