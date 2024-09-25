using Microsoft.Xna.Framework;

namespace Terraria
{
	public class HotKeyButtonHelper
	{
		public string key;

		public float width;

		public float height;

		public float x;

		public float y;

		public HotKeyButtonHelper(HotKeyButton hotKeyButton)
		{
			key = hotKeyButton.key;
			width = hotKeyButton.MySize.X;
			height = hotKeyButton.MySize.Y;
			x = hotKeyButton.MyPosition.X;
			y = hotKeyButton.MyPosition.Y;
		}

		public HotKeyButtonHelper(string k, float w, float h, float xx, float yy)
		{
			key = k;
			width = w;
			height = h;
			x = xx;
			y = yy;
		}

		public HotKeyButtonHelper()
		{
		}

		public HotKeyButton NewHotKeyButton()
		{
			return new HotKeyButton(new Vector2(width, height), key)
			{
				MyRelativePos = new Vector2(x, y)
			};
		}
	}
}
