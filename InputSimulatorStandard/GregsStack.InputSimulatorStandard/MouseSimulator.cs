using System;
using System.Drawing;
using System.Threading;
using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	public class MouseSimulator : IMouseSimulator
	{
		private readonly IInputMessageDispatcher messageDispatcher;

		public int MouseWheelClickSize { get; set; } = 120;


		public System.Drawing.Point Position
		{
			get
			{
				NativeMethods.GetCursorPos(out var lpPoint);
				return lpPoint;
			}
		}

		public MouseSimulator()
			: this(new WindowsInputMessageDispatcher())
		{
		}

		internal MouseSimulator(IInputMessageDispatcher messageDispatcher)
		{
			this.messageDispatcher = messageDispatcher ?? throw new InvalidOperationException(string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.", typeof(MouseSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(Input).Name));
		}

		public IMouseSimulator MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
		{
			Input[] inputList = new InputBuilder().AddRelativeMouseMovement(pixelDeltaX, pixelDeltaY).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator MoveMouseTo(double absoluteX, double absoluteY)
		{
			Input[] inputList = new InputBuilder().AddAbsoluteMouseMovement((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
		{
			Input[] inputList = new InputBuilder().AddAbsoluteMouseMovementOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator LeftButtonDown()
		{
			return ButtonDown(MouseButton.LeftButton);
		}

		public IMouseSimulator LeftButtonUp()
		{
			return ButtonUp(MouseButton.LeftButton);
		}

		public IMouseSimulator LeftButtonClick()
		{
			return ButtonClick(MouseButton.LeftButton);
		}

		public IMouseSimulator LeftButtonDoubleClick()
		{
			return ButtonDoubleClick(MouseButton.LeftButton);
		}

		public IMouseSimulator MiddleButtonDown()
		{
			return ButtonDown(MouseButton.MiddleButton);
		}

		public IMouseSimulator MiddleButtonUp()
		{
			return ButtonUp(MouseButton.MiddleButton);
		}

		public IMouseSimulator MiddleButtonClick()
		{
			return ButtonClick(MouseButton.MiddleButton);
		}

		public IMouseSimulator MiddleButtonDoubleClick()
		{
			return ButtonDoubleClick(MouseButton.MiddleButton);
		}

		public IMouseSimulator RightButtonDown()
		{
			return ButtonDown(MouseButton.RightButton);
		}

		public IMouseSimulator RightButtonUp()
		{
			return ButtonUp(MouseButton.RightButton);
		}

		public IMouseSimulator RightButtonClick()
		{
			return ButtonClick(MouseButton.RightButton);
		}

		public IMouseSimulator RightButtonDoubleClick()
		{
			return ButtonDoubleClick(MouseButton.RightButton);
		}

		public IMouseSimulator XButtonDown(int buttonId)
		{
			Input[] inputList = new InputBuilder().AddMouseXButtonDown(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonUp(int buttonId)
		{
			Input[] inputList = new InputBuilder().AddMouseXButtonUp(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonClick(int buttonId)
		{
			Input[] inputList = new InputBuilder().AddMouseXButtonClick(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator XButtonDoubleClick(int buttonId)
		{
			Input[] inputList = new InputBuilder().AddMouseXButtonDoubleClick(buttonId).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator VerticalScroll(int scrollAmountInClicks)
		{
			Input[] inputList = new InputBuilder().AddMouseVerticalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator HorizontalScroll(int scrollAmountInClicks)
		{
			Input[] inputList = new InputBuilder().AddMouseHorizontalWheelScroll(scrollAmountInClicks * MouseWheelClickSize).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		public IMouseSimulator Sleep(int millisecondsTimeout)
		{
			return Sleep(TimeSpan.FromMilliseconds(millisecondsTimeout));
		}

		public IMouseSimulator Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		private void SendSimulatedInput(Input[] inputList)
		{
			messageDispatcher.DispatchInput(inputList);
		}

		private IMouseSimulator ButtonDown(MouseButton mouseButton)
		{
			Input[] inputList = new InputBuilder().AddMouseButtonDown(mouseButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		private IMouseSimulator ButtonUp(MouseButton mouseButton)
		{
			Input[] inputList = new InputBuilder().AddMouseButtonUp(mouseButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		private IMouseSimulator ButtonClick(MouseButton mouseButton)
		{
			Input[] inputList = new InputBuilder().AddMouseButtonClick(mouseButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		private IMouseSimulator ButtonDoubleClick(MouseButton mouseButton)
		{
			Input[] inputList = new InputBuilder().AddMouseButtonDoubleClick(mouseButton).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}
	}
}
