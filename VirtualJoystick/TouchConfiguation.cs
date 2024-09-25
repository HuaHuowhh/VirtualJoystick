using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.ModLoader;

namespace Terraria
{
	public static class TouchConfiguation
	{
		public static string path = System.IO.Path.Combine(Main.SavePath ,"touchHotKeys.json");

		public static float LeftStickScale;

		public static float RightStickScale;

		internal static List<HotKeyButton> hotKeyButtons;

		public static bool Editing;

		public static bool NeedToRemove;

		public static bool NeedToScaleUp;

		public static bool NeedToScaleDown;

		public static Vector2 LeftStickSize => new Vector2(127f, 127f) * LeftStickScale;

		public static Vector2 RightStickSize => new Vector2(127f, 127f) * RightStickScale;

		public static void Initialize()
		{
			LeftStickScale = 1f;
			RightStickScale = 1f;
			hotKeyButtons = new List<HotKeyButton>();
		}

		public static void Save()
		{
			if (!File.Exists(path))
			{
				File.Create(path);
			}
			try
			{
				StreamWriter writer = new StreamWriter(path);
				try
				{
					writer.WriteLine(JsonConvert.SerializeObject(new StickSizeHelper
					{
						LeftJoyStickScale = LeftStickScale,
						RightJoyStickScale = RightStickScale
					}));
					hotKeyButtons.ForEach(delegate(HotKeyButton h)
					{
						writer.WriteLine(JsonConvert.SerializeObject(new HotKeyButtonHelper(h)));
					});
					writer.Close();
				}
				finally
				{
					if (writer != null)
					{
						((IDisposable)writer).Dispose();
					}
				}
			}
			catch (Exception message)
			{
			//	Logging.tML.Debug(message);
			}
		}

		public static void Load()
		{
			try
			{
				if (!File.Exists(path))
				{
					return;
				}
				using StreamReader streamReader = new StreamReader(path);
				string value = streamReader.ReadLine();
				JsonConvert.DeserializeObject<StickSizeHelper>(value).Set();
				while (true)
				{
					string text = streamReader.ReadLine();
					if (text == "" || JsonConvert.DeserializeObject<HotKeyButtonHelper>(text) == null)
					{
						break;
					}
					hotKeyButtons.Add(JsonConvert.DeserializeObject<HotKeyButtonHelper>(text).NewHotKeyButton());
				}
				streamReader.Close();
			}
			catch (Exception message)
			{
				//Logging.tML.Debug(message);
			}
		}
	}
}
