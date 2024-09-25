using System;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria
{
	public class PowerUIItemSlot : PowerUIPanel
	{
		public Item MyItem;

		public bool UseSizeReact;

		private Vector2 MyAltSize;

		private float SizeFix;

		private bool active;
	//	public LegacySoundPlayer legacySoundPlayer;
        public PowerUIItemSlot()
		{
			UseSizeReact = true;
			MyItem = new Item();
			MySize = new Vector2(52f, 52f);
			MyColor = new Color(63, 82, 151) * 0.7f;
			MouseClickMe = (Action)Delegate.Combine(MouseClickMe, new Action(ClickMe));
			MouseUponMe = (Action)Delegate.Combine(MouseUponMe, new Action(UponMe));
			MouseNotUponMe = (Action)Delegate.Combine(MouseNotUponMe, new Action(NotUponMe));
			SizeFix = 1f;
		}

		public void ClickMe()
		{
			if (Main.mouseItem.type != 0)
			{
				Item myItem = MyItem;
				MyItem = Main.mouseItem;
				Main.mouseItem = myItem;
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
			else if (Main.mouseItem.type == 0 && MyItem.type != 0)
			{
				Main.mouseItem = MyItem;
				MyItem = new Item();
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
		}

		public void UponMe()
		{
			active = Main.mouseItem.type != 0;
			Main.mouseText = true;
		}

		public void NotUponMe()
		{
			active = false;
		}

		public override void PostActive()
		{
			MyAltSize = MySize * SizeFix;
			if (UseSizeReact)
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
			MyCenterFix = MyAltSize;
			MyCenterFix.X *= ((MyCenterType.X == 1) ? 0f : ((MyCenterType.X == 2) ? 0.5f : 1f));
			MyCenterFix.Y *= ((MyCenterType.Y == 1) ? 0f : ((MyCenterType.Y == 2) ? 0.5f : 1f));
			MyCenterFix = new Vector2((int)Math.Floor(MyCenterFix.X), (int)Math.Floor(MyCenterFix.Y));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Utils.DrawInvBG(spriteBatch, MyPosition.X - MyCenterFix.X, MyPosition.Y - MyCenterFix.Y, MyAltSize.X, MyAltSize.Y, MyColor);
            if (MyItem.type != 0)
			{
                Main.GetItemDrawFrame(MyItem.type, out var itemTexture, out var rectangle);
               // Texture2D texture2D = Main.itemTexture[MyItem.type];
				float num = 0.8f * SizeFix;
				if (itemTexture.Width > 42)
				{
					num *= 42f / (float)itemTexture.Width;
				}
				else if (itemTexture.Height > 42)
				{
					num *= 42f / (float)itemTexture.Height;
				}
				spriteBatch.Draw(itemTexture, MyPosition - MyCenterFix + new Vector2(26f, 26f) * SizeFix - itemTexture.Size() * num * 0.5f, null, Color.White, 0f, Vector2.Zero, num, SpriteEffects.None, 0f);
				if (MyItem.stack != 1)
				{
					Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, MyItem.stack.ToString(), MyPosition.X - MyCenterFix.X + 6f * SizeFix, MyPosition.Y - MyCenterFix.Y + 32f * SizeFix, Color.White, Color.Black, Vector2.Zero, 0.8f * SizeFix);
				}
			}
			DrawChildren(spriteBatch);
		}
	}
}
