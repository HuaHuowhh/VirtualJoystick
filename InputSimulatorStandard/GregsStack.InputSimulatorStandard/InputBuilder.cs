using System;
using System.Collections;
using System.Collections.Generic;
using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	internal class InputBuilder : IEnumerable<Input>, IEnumerable
	{
		private readonly List<Input> inputList;

		private static readonly List<VirtualKeyCode> ExtendedKeys = new List<VirtualKeyCode>
		{
			VirtualKeyCode.NUMPAD_RETURN,
			VirtualKeyCode.MENU,
			VirtualKeyCode.RMENU,
			VirtualKeyCode.CONTROL,
			VirtualKeyCode.RCONTROL,
			VirtualKeyCode.INSERT,
			VirtualKeyCode.DELETE,
			VirtualKeyCode.HOME,
			VirtualKeyCode.END,
			VirtualKeyCode.PRIOR,
			VirtualKeyCode.NEXT,
			VirtualKeyCode.RIGHT,
			VirtualKeyCode.UP,
			VirtualKeyCode.LEFT,
			VirtualKeyCode.DOWN,
			VirtualKeyCode.NUMLOCK,
			VirtualKeyCode.CANCEL,
			VirtualKeyCode.SNAPSHOT,
			VirtualKeyCode.DIVIDE
		};

		public Input this[int position] => inputList[position];

		public InputBuilder()
		{
			inputList = new List<Input>();
		}

		public Input[] ToArray()
		{
			return inputList.ToArray();
		}

		public IEnumerator<Input> GetEnumerator()
		{
			return inputList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public static bool IsExtendedKey(VirtualKeyCode keyCode)
		{
			return ExtendedKeys.Contains(keyCode);
		}

		public InputBuilder AddKeyDown(VirtualKeyCode keyCode)
		{
			ushort num = (ushort)(keyCode & (VirtualKeyCode)65535);
			Input input = default(Input);
			input.Type = 1u;
			input.Data.Keyboard = new KeyboardInput
			{
				KeyCode = num,
				Scan = (ushort)(NativeMethods.MapVirtualKey(num, 0u) & 0xFFu),
				Flags = (IsExtendedKey(keyCode) ? 1u : 0u),
				Time = 0u,
				ExtraInfo = IntPtr.Zero
			};
			Input item = input;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddKeyUp(VirtualKeyCode keyCode)
		{
			ushort num = (ushort)(keyCode & (VirtualKeyCode)65535);
			Input input = default(Input);
			input.Type = 1u;
			input.Data.Keyboard = new KeyboardInput
			{
				KeyCode = num,
				Scan = (ushort)(NativeMethods.MapVirtualKey(num, 0u) & 0xFFu),
				Flags = (IsExtendedKey(keyCode) ? 3u : 2u),
				Time = 0u,
				ExtraInfo = IntPtr.Zero
			};
			Input item = input;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddKeyPress(VirtualKeyCode keyCode)
		{
			AddKeyDown(keyCode);
			AddKeyUp(keyCode);
			return this;
		}

		public InputBuilder AddCharacter(char character)
		{
			ushort num = character;
			Input input = default(Input);
			input.Type = 1u;
			input.Data.Keyboard = new KeyboardInput
			{
				KeyCode = 0,
				Scan = num,
				Flags = 4u,
				Time = 0u,
				ExtraInfo = IntPtr.Zero
			};
			Input item = input;
			input = default(Input);
			input.Type = 1u;
			input.Data.Keyboard = new KeyboardInput
			{
				KeyCode = 0,
				Scan = num,
				Flags = 6u,
				Time = 0u,
				ExtraInfo = IntPtr.Zero
			};
			Input item2 = input;
			if ((num & 0xFF00) == 57344)
			{
				item.Data.Keyboard.Flags |= 1u;
				item2.Data.Keyboard.Flags |= 1u;
			}
			inputList.Add(item);
			inputList.Add(item2);
			return this;
		}

		public InputBuilder AddCharacters(IEnumerable<char> characters)
		{
			foreach (char character in characters)
			{
				AddCharacter(character);
			}
			return this;
		}

		public InputBuilder AddCharacters(string characters)
		{
			return AddCharacters(characters.ToCharArray());
		}

		public InputBuilder AddRelativeMouseMovement(int x, int y)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 1u;
			item.Data.Mouse.X = x;
			item.Data.Mouse.Y = y;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 32769u;
			item.Data.Mouse.X = (int)(absoluteX * 65536 / NativeMethods.GetSystemMetrics(0u));
			item.Data.Mouse.Y = (int)(absoluteY * 65536 / NativeMethods.GetSystemMetrics(1u));
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 49153u;
			item.Data.Mouse.X = absoluteX;
			item.Data.Mouse.Y = absoluteY;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonDown(MouseButton button)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = (uint)ToMouseButtonDownFlag(button);
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseXButtonDown(int xButtonId)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 128u;
			item.Data.Mouse.MouseData = (uint)xButtonId;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonUp(MouseButton button)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = (uint)ToMouseButtonUpFlag(button);
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseXButtonUp(int xButtonId)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 256u;
			item.Data.Mouse.MouseData = (uint)xButtonId;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseButtonClick(MouseButton button)
		{
			return AddMouseButtonDown(button).AddMouseButtonUp(button);
		}

		public InputBuilder AddMouseXButtonClick(int xButtonId)
		{
			return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
		}

		public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
		{
			return AddMouseButtonClick(button).AddMouseButtonClick(button);
		}

		public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
		{
			return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
		}

		public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 2048u;
			item.Data.Mouse.MouseData = (uint)scrollAmount;
			inputList.Add(item);
			return this;
		}

		public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
		{
			Input input = default(Input);
			input.Type = 0u;
			Input item = input;
			item.Data.Mouse.Flags = 4096u;
			item.Data.Mouse.MouseData = (uint)scrollAmount;
			inputList.Add(item);
			return this;
		}

		private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
		{
			switch (button)
			{
			case MouseButton.LeftButton:
				return MouseFlag.LeftDown;
			case MouseButton.MiddleButton:
				return MouseFlag.MiddleDown;
			case MouseButton.RightButton:
				return MouseFlag.RightDown;
			default:
				return MouseFlag.LeftDown;
			}
		}

		private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
		{
			switch (button)
			{
			case MouseButton.LeftButton:
				return MouseFlag.LeftUp;
			case MouseButton.MiddleButton:
				return MouseFlag.MiddleUp;
			case MouseButton.RightButton:
				return MouseFlag.RightUp;
			default:
				return MouseFlag.LeftUp;
			}
		}
	}
}
