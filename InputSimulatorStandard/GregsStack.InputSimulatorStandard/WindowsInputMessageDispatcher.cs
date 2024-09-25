using System;
using System.Runtime.InteropServices;
using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	internal class WindowsInputMessageDispatcher : IInputMessageDispatcher
	{
		public void DispatchInput(Input[] inputs)
		{
			if (inputs == null)
			{
				throw new ArgumentNullException("inputs");
			}
			if (inputs.Length == 0)
			{
				throw new ArgumentException("The input array was empty", "inputs");
			}
			if (NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input))) != inputs.Length)
			{
				throw new Exception("Some simulated input commands were not sent successfully. The most common reason for this happening are the security features of Windows including User Interface Privacy Isolation (UIPI). Your application can only send commands to applications of the same or lower elevation. Similarly certain commands are restricted to Accessibility/UIAutomation applications. Refer to the project home page and the code samples for more information.");
			}
		}
	}
}
