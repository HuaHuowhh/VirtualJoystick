using FNATouch;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.RGB;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace VirtualJoystick
{
	public class VirtualJoystick : Mod
	{
        internal VerticalInventory verticalInventory;

        public override void Load()
		{
            verticalInventory = new VerticalInventory();
            verticalInventory.Activate(); // º§ªÓUI
            base.Load();
            

        }
     
        public override void Unload()
        {
           
        }
       
        
    }
}