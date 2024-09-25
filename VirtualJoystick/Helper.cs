using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria
{
    internal class Helper
    {
        public static Vector2 Interpolation(float x, float y, float targetx, float targety, float i, float maxi)
        {
            x *= maxi - i;
            x /= maxi;
            y *= maxi - i;
            y /= maxi;
            targetx *= i;
            targetx /= maxi;
            targety *= i;
            targety /= maxi;
            return new Vector2(x + targetx, y + targety);
        }

        public static Vector2 Interpolation(Vector2 current, Vector2 target, float i, float maxi)
        {
            current *= maxi - i;
            current /= maxi;
            target *= i;
            target /= maxi;
            return current + target;
        }

        public static Color InterpolationColor(Color current, Color target, float i, float maxi)
        {
            float num = (int)current.R;
            float num2 = (int)current.G;
            float num3 = (int)current.B;
            float num4 = (int)current.A;
            float num5 = (int)target.R;
            float num6 = (int)target.G;
            float num7 = (int)target.B;
            float num8 = (int)target.A;
            num *= maxi - i;
            num /= maxi;
            num2 *= maxi - i;
            num2 /= maxi;
            num3 *= maxi - i;
            num3 /= maxi;
            num4 *= maxi - i;
            num4 /= maxi;
            num5 *= i;
            num5 /= maxi;
            num6 *= i;
            num6 /= maxi;
            num7 *= i;
            num7 /= maxi;
            num8 *= i;
            num8 /= maxi;
            return new Color((int)(num + num5), (int)(num2 + num6), (int)(num3 + num7), (int)(num4 + num8));
        }

        public static double InterpolationSingle(double current, double target, double i, double maxi)
        {
            current *= (maxi - i) / maxi;
            target *= i / maxi;
            return current + target;
        }

        public static Color GetRainbowColorLinear(int i, int imax)
        {
            float num = imax / 7;
            if ((float)i <= num)
            {
                return InterpolationColor(Color.Red, Color.Orange, i, num);
            }
            if ((float)i <= 2f * num && (float)i > num)
            {
                return InterpolationColor(Color.Orange, Color.Yellow, (float)i - num, num);
            }
            if ((float)i <= 3f * num && (float)i > 2f * num)
            {
                return InterpolationColor(Color.Yellow, Color.Green, (float)i - 2f * num, num);
            }
            if ((float)i <= 4f * num && (float)i > 3f * num)
            {
                return InterpolationColor(Color.Green, Color.Cyan, (float)i - 3f * num, num);
            }
            if ((float)i <= 5f * num && (float)i > 4f * num)
            {
                return InterpolationColor(Color.Cyan, Color.Blue, (float)i - 4f * num, num);
            }
            if ((float)i <= 6f * num && (float)i > 5f * num)
            {
                return InterpolationColor(Color.Blue, Color.Purple, (float)i - 5f * num, num);
            }
            if ((float)i > 6f * num)
            {
                return InterpolationColor(Color.Purple, Color.Red, (float)i - 6f * num, num);
            }
            return Color.White;
        }

        public static Color ToGreyColor(Color color)
        {
            int num = color.R + color.G + color.B;
            num /= 3;
            return new Color(num, num, num);
        }

        public static Vector2 RelativePositionTrans(Vector2 current, Vector2 target)
        {
            return new Vector2(current.X + target.X, current.Y + target.Y);
        }

        public static Vector2 ToUnitVector(Vector2 vector)
        {
            return vector / vector.Length();
        }

        public static Vector2 ToUnitVector(float x, float y)
        {
            return new Vector2(x, y) / new Vector2(x, y).Length();
        }

        public static float HyperPlus(float[] a, int b, float c)
        {
            for (int i = 0; i <= b; i++)
            {
                c += a[i];
            }
            return c;
        }

        public static Vector2 HyperPlus(Vector2[] a, int b, Vector2 c)
        {
            for (int i = 0; i <= b; i++)
            {
                c += a[i];
            }
            return c;
        }

        public static void LifeSteal(Player player, int damage, float ratio)
        {
            damage = (int)((float)damage * ratio);
            if (damage == 0)
            {
                damage = 1;
            }
            if (damage + player.statLife >= player.statLifeMax2 && player.statLifeMax2 >= player.statLife)
            {
                damage = player.statLifeMax2 - player.statLife;
            }
            if (damage != 0)
            {
                player.statLife += damage;
                player.HealEffect(damage);
            }
        }

        public static Point ToTilesPos(Vector2 position)
        {
            return new Point((int)(position.X / 16f), (int)(position.Y / 16f));
        }

        public static float GetStringLength(DynamicSpriteFont font, string text, float scale)
        {
            return font.MeasureString(text).X * scale;
        }

        public static float GetStringHeight(DynamicSpriteFont font, string text, float scale)
        {
            return font.MeasureString(text).Y * scale;
        }

        public static Vector2 GetStringSize(DynamicSpriteFont font, string text, float scale)
        {
            return font.MeasureString(text) * scale;
        }

        public static bool CanShowExtraUI()
        {
            return !Main.mapFullscreen && !Main.ingameOptionsWindow && !Main.inFancyUI && !Main.playerInventory;
        }

       

        public static bool InTheRange(Vector2 current, Vector2 range, Vector2 target)
        {
            if (range.X < 0f)
            {
                return false;
            }
            if (range.Y < 0f)
            {
                return false;
            }
            if (current.X > target.X)
            {
                return false;
            }
            if (current.X + range.X < target.X)
            {
                return false;
            }
            if (current.Y > target.Y)
            {
                return false;
            }
            if (current.Y + range.Y < target.Y)
            {
                return false;
            }
            return true;
        }
    }
}
