using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.ID;

namespace Terraria
{
	public class PowerUIButton : PowerUIPanel
	{
		public bool UseColorReact;

		private float SizeFix;

		private bool active;
	//	public LegacySoundPlayer soundEffectInstance;
		public PowerUIButton()
		{
          
            UseColorReact = true;
			MySize = Vector2.Zero;
			MyColor = new Color(63, 82, 151) * 0.7f;
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

		public override void PostActive()
		{
			if (UseColorReact)
			{
				if (active && SizeFix < 1.08f)
				{
					SizeFix += 0.02f;
				}
				if (!active && SizeFix > 1f)
				{
					SizeFix -= 0.02f;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (MyTexture != null)
			{
				Draw(spriteBatch);
			}
			else
			{
                Utils.DrawInvBG(spriteBatch, MyPosition.X - MyCenterFix.X, MyPosition.Y - MyCenterFix.Y, MySize.X, MySize.Y, Helper.InterpolationColor(MyColor * (255f / (float)(int)MyColor.A), Color.White, SizeFix - 1f, 0.65f) * ((float)(int)MyColor.A / 255f));
            }
            DrawChildren(spriteBatch);
		}
	}
}
