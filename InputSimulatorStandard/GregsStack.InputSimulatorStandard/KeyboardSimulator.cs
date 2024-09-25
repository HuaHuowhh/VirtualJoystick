using System;
using System.Collections.Generic;
using System.Threading;
using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	public class KeyboardSimulator : IKeyboardSimulator
	{
		private readonly IInputMessageDispatcher messageDispatcher;

		public KeyboardSimulator()
			: this(new WindowsInputMessageDispatcher())
		{
		}

		internal KeyboardSimulator(IInputMessageDispatcher messageDispatcher)
		{
			this.messageDispatcher = messageDispatcher ?? throw new InvalidOperationException(string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.", typeof(KeyboardSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(Input).Name));
		}

		public IKeyboardSimulator KeyDown(VirtualKeyCode keyCode)
		{
			Input[] inputList = new InputBuilder().AddKeyDown(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyUp(VirtualKeyCode keyCode)
		{
			Input[] inputList = new InputBuilder().AddKeyUp(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyPress(VirtualKeyCode keyCode)
		{
			Input[] inputList = new InputBuilder().AddKeyPress(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator KeyPress(params VirtualKeyCode[] keyCodes)
		{
			InputBuilder inputBuilder = new InputBuilder();
			KeysPress(inputBuilder, keyCodes);
			SendSimulatedInput(inputBuilder.ToArray());
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
		{
			ModifiedKeyStroke(new VirtualKeyCode[1] { modifierKeyCode }, new VirtualKeyCode[1] { keyCode });
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
		{
			ModifiedKeyStroke(modifierKeyCodes, new VirtualKeyCode[1] { keyCode });
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
		{
			ModifiedKeyStroke(new VirtualKeyCode[1] { modifierKey }, keyCodes);
			return this;
		}

		public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
		{
			InputBuilder inputBuilder = new InputBuilder();
			ModifiersDown(inputBuilder, modifierKeyCodes);
			KeysPress(inputBuilder, keyCodes);
			ModifiersUp(inputBuilder, modifierKeyCodes);
			SendSimulatedInput(inputBuilder.ToArray());
			return this;
		}

		public IKeyboardSimulator TextEntry(string text)
		{
			if ((long)text.Length > 2147483647L)
			{
				throw new ArgumentException($"The text parameter is too long. It must be less than {2147483647u} characters.", "text");
			}
			Input[] inputList = new InputBuilder().AddCharacters(text).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator TextEntry(char character)
		{
			Input[] inputList = new InputBuilder().AddCharacter(character).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IKeyboardSimulator Sleep(int millisecondsTimeout)
		{
			return Sleep(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public IKeyboardSimulator Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		private void ModifiersDown(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
		{
			if (modifierKeyCodes == null)
			{
				return;
			}
			foreach (VirtualKeyCode modifierKeyCode in modifierKeyCodes)
			{
				builder.AddKeyDown(modifierKeyCode);
			}
		}

		private void ModifiersUp(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
		{
			if (modifierKeyCodes != null)
			{
				Stack<VirtualKeyCode> stack = new Stack<VirtualKeyCode>(modifierKeyCodes);
				while (stack.Count > 0)
				{
					builder.AddKeyUp(stack.Pop());
				}
			}
		}

		private void KeysPress(InputBuilder builder, IEnumerable<VirtualKeyCode> keyCodes)
		{
			if (keyCodes == null)
			{
				return;
			}
			foreach (VirtualKeyCode keyCode in keyCodes)
			{
				builder.AddKeyPress(keyCode);
			}
		}

		private void SendSimulatedInput(Input[] inputList)
		{
			messageDispatcher.DispatchInput(inputList);
		}
	}
}
