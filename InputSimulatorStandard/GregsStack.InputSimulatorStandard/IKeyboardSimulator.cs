using System;
using System.Collections.Generic;
using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	public interface IKeyboardSimulator
	{
		IKeyboardSimulator KeyDown(VirtualKeyCode keyCode);

		IKeyboardSimulator KeyPress(VirtualKeyCode keyCode);

		IKeyboardSimulator KeyPress(params VirtualKeyCode[] keyCodes);

		IKeyboardSimulator KeyUp(VirtualKeyCode keyCode);

		IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes);

		IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode);

		IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes);

		IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode);

		IKeyboardSimulator TextEntry(string text);

		IKeyboardSimulator TextEntry(char character);

		IKeyboardSimulator Sleep(int millisecondsTimeout);

		IKeyboardSimulator Sleep(TimeSpan timeout);
	}
}
