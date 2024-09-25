using GregsStack.InputSimulatorStandard.Native;

namespace GregsStack.InputSimulatorStandard
{
	internal interface IInputMessageDispatcher
	{
		void DispatchInput(Input[] inputs);
	}
}
