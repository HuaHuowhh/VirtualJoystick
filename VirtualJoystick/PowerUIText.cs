using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Terraria
{
    public class PowerUIText : PowerUIElement
    {
        public string MyText;

        public float MyScale;

        public Color MyMainColor;
        //public CenterType MyCenterType;

        public Color MyBorderColor;

        public DynamicSpriteFont MyFont;

        public PowerUIText()
        {
            MyText = "";

            MyScale = 1f;
            MyMainColor = Color.White;
            MyBorderColor = Color.Black;
            MyFont = FontAssets.DeathText.Value;
        }

        public override void PreActive()
        {
            try
            {
                MySize = Helper.GetStringSize(MyFont, MyText, MyScale);
            }
            catch (Exception o)
            {
                Main.NewText(o);
            }
        }
        public Vector2 GetTextSize()
        {
            return MyFont.MeasureString(MyText) * MyScale;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(MyFont, MyText, position, Color.White, 0f, Vector2.Zero, MyScale, SpriteEffects.None, 0f);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Utils.DrawBorderStringFourWay(spriteBatch, MyFont, MyText, MyPosition.X - MyCenterFix.X, MyPosition.Y - MyCenterFix.Y, MyMainColor, MyBorderColor, Vector2.Zero, MyScale);
            DrawChildren(spriteBatch);
        }
    }
}
