using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;

namespace Terraria
{
	public class PowerUITextButton : PowerUIText
	{
		public bool UseColorReact;

		public bool UseSizeReact;

		private Color MyAltColor;

		private float SizeFix;

		private bool active;
		//public LegacySoundPlayer legacySoundPlayer;
        public PowerUITextButton()
		{
			UseColorReact = true;
			UseSizeReact = true;
			MyAltColor = Color.White;
			MouseClickMe = (Action)Delegate.Combine(MouseClickMe, new Action(ClickMe));
			MouseUponMe = (Action)Delegate.Combine(MouseUponMe, new Action(UponMe));
			MouseNotUponMe = (Action)Delegate.Combine(MouseNotUponMe, new Action(NotUponMe));
			SizeFix = 1f;
		}

		public void ClickMe()
		{
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

		public void UponMe()
		{
			if (!active)
			{
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
			active = true;
		}

		public void NotUponMe()
		{
			active = false;
		}

		public override void PreActive()
		{
			if (UseColorReact)
			{
				if (active && SizeFix < 1.12f)
				{
					SizeFix += 0.03f;
				}
				if (!active && SizeFix > 1f)
				{
					SizeFix -= 0.03f;
				}
			}
			MyAltColor = Helper.InterpolationColor(MyMainColor, Color.Gray, 1.12f - (UseColorReact ? SizeFix : 1.12f), 0.12f);
			MySize = Helper.GetStringSize(MyFont, MyText, MyScale * (UseSizeReact ? SizeFix : 1f));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Utils.DrawBorderStringFourWay(spriteBatch, MyFont, MyText, MyPosition.X - MyCenterFix.X, MyPosition.Y - MyCenterFix.Y, MyAltColor, MyBorderColor, Vector2.Zero, MyScale * SizeFix);
			DrawChildren(spriteBatch);
		}
	}
}
