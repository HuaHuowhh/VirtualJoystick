using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria
{
	public class PowerUIAltPanel : PowerUIElement
	{
		public Color MyMainColor;

		public Color MyBorderColor;

		public PowerUIAltPanel()
		{
			MyMainColor = Color.White;
			MyBorderColor = Color.Black;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, MyPosition - MyCenterFix + new Vector2(2f, MySize.Y), new Rectangle(0, 0, (int)MySize.X - 2, 2), MyBorderColor);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, MyPosition - MyCenterFix + new Vector2(2f, 0f), new Rectangle(0, 0, (int)MySize.X - 2, 2), MyBorderColor);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, MyPosition - MyCenterFix + new Vector2(0f, 2f), new Rectangle(0, 0, 2, (int)MySize.Y - 2), MyBorderColor);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, MyPosition - MyCenterFix + new Vector2(MySize.X, 2f), new Rectangle(0, 0, 2, (int)MySize.Y - 2), MyBorderColor);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, MyPosition - MyCenterFix + new Vector2(2f, 2f), new Rectangle(2, 2, (int)MySize.X - 2, (int)MySize.Y - 2), MyMainColor);
			DrawChildren(spriteBatch);
		}
	}
}
