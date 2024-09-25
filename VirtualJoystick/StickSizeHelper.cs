namespace Terraria
{
	public class StickSizeHelper
	{
		public float LeftJoyStickScale;

		public float RightJoyStickScale;

		public void Set()
		{
			TouchConfiguation.LeftStickScale = LeftJoyStickScale;
			TouchConfiguation.RightStickScale = RightJoyStickScale;
		}
	}
}
