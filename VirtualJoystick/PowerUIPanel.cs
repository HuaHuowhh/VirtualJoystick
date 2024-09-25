using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria
{
	public class PowerUIPanel : PowerUIElement
	{
		public Color MyColor;

		public PowerUIPanel()
		{
			MyColor = Color.White;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Utils.DrawInvBG(spriteBatch, MyPosition.X - MyCenterFix.X, MyPosition.Y - MyCenterFix.Y, MySize.X, MySize.Y, MyColor);
			DrawChildren(spriteBatch);
		}
	}
}
