using Microsoft.Xna.Framework;

namespace Terraria
{
	public abstract class PowerUI
	{
		protected readonly Vector2 ScreenCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;

		protected readonly Vector2 ScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);

		protected const int vesion = 1003;
	}
}
