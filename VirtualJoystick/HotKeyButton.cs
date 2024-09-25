using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Terraria.GameContent;
using Terraria.GameInput;

namespace Terraria
{
    public class HotKeyButton : PowerUIButton
    {
        public string key;
        public int pressTimer;

        public HotKeyButton(Vector2 size, string Key = "Space")
        {
            key = Key;
            UseColorReact = false;
            UseMouseFix = false;
            MyCenterType = CenterType.MiddleCenter;
            MySize = size;
            MouseClickMe = delegate
            {
                if (TouchConfiguation.Editing)
                {
                    if (TouchConfiguation.NeedToRemove)
                    {
                        TouchConfiguation.hotKeyButtons.Remove(this);
                        TouchConfiguation.NeedToRemove = false;
                    }
                    if (TouchConfiguation.NeedToScaleUp)
                    {
                        MySize += new Vector2(10f, 10f);
                        TouchConfiguation.NeedToScaleUp = false;
                        RefreshText();
                    }
                    if (TouchConfiguation.NeedToScaleDown)
                    {
                        MySize -= ((MySize.X > 10f) ? new Vector2(10f, 10f) : Vector2.Zero);
                        TouchConfiguation.NeedToScaleDown = false;
                        RefreshText();
                    }

                    var touchCollection = (TouchCollection)TouchSupport.touchSupport.GetType().GetProperty("Collection", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(TouchSupport.touchSupport);
                    if (touchCollection.Count > 0)
                    {
                        MyRelativePos = touchCollection[0].Position / Main.UIScale;
                    }
                    pressTimer += 4;
                }
                else
                {
                    MyColor = Helper.InterpolationColor(MyColor, new Color(255, 230, 30), 1f, 2f);
                    Main.LocalPlayer.mouseInterface = true;
                }
            };
            MouseUponMe = (Action)Delegate.Combine(MouseUponMe, (Action)delegate
            {
                MyColor = Helper.InterpolationColor(MyColor, new Color(255, 230, 30), 1f, 2f);
                Main.LocalPlayer.mouseInterface = true;
            });
            MouseNotUponMe = (Action)Delegate.Combine(MouseNotUponMe, (Action)delegate
            {
                MyColor = Helper.InterpolationColor(MyColor, new Color(63, 82, 151) * 0.7f, 1f, 2f);
                if (TouchConfiguation.Editing && pressTimer > 0)
                {
                    pressTimer--;
                    var touchCollection = (TouchCollection)TouchSupport.touchSupport.GetType().GetProperty("Collection", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(TouchSupport.touchSupport);
                    if (touchCollection.Count > 0)
                    {
                        MyRelativePos = touchCollection[0].Position / Main.UIScale;
                    }
                }
            });
            PowerUIText powerUIText = new PowerUIText
            {
                MyText = key,
                MyScale = 0.6f
            };
            if (Helper.GetStringLength(FontAssets.DeathText.Value, key, 0.6f) > size.X)
            {
                powerUIText.MyScale *= size.X / Helper.GetStringLength(FontAssets.DeathText.Value, key, 0.6f);
            }
            powerUIText.MyCenterType = CenterType.MiddleCenter;
            powerUIText.MyRelativePos.Y += Helper.GetStringHeight(FontAssets.DeathText.Value, key, powerUIText.MyScale) / 8f;
            Append(powerUIText);
        }

        public void RefreshText()
        {
            PowerUIText powerUIText = children[0] as PowerUIText;
            powerUIText.MyScale = 0.6f;
            if (Helper.GetStringLength(FontAssets.DeathText.Value, key, 0.6f) > MySize.X)
            {
                powerUIText.MyScale *= MySize.X / Helper.GetStringLength(FontAssets.DeathText.Value, key, 0.6f);
            }
            powerUIText.MyRelativePos = Vector2.Zero;
            powerUIText.MyRelativePos.Y += Helper.GetStringHeight(FontAssets.DeathText.Value, key, powerUIText.MyScale) / 8f;
        }
    }
}
