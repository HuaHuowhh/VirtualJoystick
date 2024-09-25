using DigitalHijinks.MiKeys.Managers;
using FNATouch;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoMod.Core.Utils;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using static DigitalHijinks.MiKeys.Helpers.Globals;
namespace LansUILib
{
    public  class UISystem : ModSystem
    {


        //Joystick joystick;
        public static SpriteFont SF { get; set; }
        private static UISystem _instance = null;
        public EntryManager EM { get; set; }
        public static DynamicSpriteFont fonts;
        string resultString = "";
        public static UISystem Instance
        {
            get { return _instance; }
        }

        public override void Load()
        {
            _instance = this;
            base.Load();









            Joystick.LoadContent();

            EM = new EntryManager();
          

        }

        public override void Unload()
        {
            base.Unload();
          
        }

     

        void InputCaptured(object sender, EventArgs e)
        {
            resultString = sender as string;
            EM.InputCaptured -= InputCaptured;
        }
        public void InitialSetup()
        {

            CM = Main.instance.Content;
          
          //  GDM = GDM;
            GFX = Main.instance.GraphicsDevice;
            VPort = Main.instance.GraphicsDevice.Viewport;

          
            Main.instance.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
           // VPort.Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 128;
           // VPort.Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 256;
            GDM.PreferredBackBufferWidth = ScreenWidth;
            GDM.PreferredBackBufferHeight = ScreenHeight;

            Main.instance.GraphicsDevice.Viewport = VPort;
            GDM.ApplyChanges();
        }
        public  void OpenKeyboard()
        {
            EM.SetupInput(gameState, 0, 3, 15);  // 设置键盘输入参数
            EM.InputCaptured += InputCaptured;  // 添加事件监听，用于处理输入
            Main.NewText("键盘已打开");  // 可选：显示一个文本提示，确认键盘已打开
        }
        public override void UpdateUI(GameTime gameTime)
        {
            SF = ModContent.Request<SpriteFont>("VirtualJoystick/Fonts/FSEX302").Value;
            EM.Load(ModContent.Request<Texture2D>("VirtualJoystick/KeyExample").Value);
            InitialSetup();
            RefreshKeyboardAndMouse();
            RefreshGamepads();

            if (EM.IsActive)
            {
                EM.Update(gameTime);
            }
            else
            {
                if (GetKeyTap(Keys.Space))
                {
                  
                }
            }
         
          

             VirtualGamepad.InitializeGraphics();

            Joystick.Update();
         
            base.UpdateUI(gameTime);


        }
       

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {








            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex + 1, new LegacyGameInterfaceLayer(
                    "LansUiLib: Virtual Joysticks",
                    () =>
                    {









                      
                        if (!Main.gameMenu)
                        {
                            float resultScale = 3f;

                            EM.Draw(Main.spriteBatch);

                            EM.DrawKeyboard(Main.spriteBatch);

                            //VirtualGamepad.DrawCircleButton(Main.spriteBatch);
                            SBX.DrawString(SF, resultString, new Vector2(HalfScreenWidth - (resultString.Length * (8 * resultScale) / 2), HalfScreenHeight - 32), Color.White, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                            VirtualGamepad.Draw(Main.spriteBatch);

                            // KeyboardInputFNA.Draw(Main.spriteBatch);
                            //Joystick.LoadContent();
                          //  Joystick.Update();
                            Joystick.Draw(Main.spriteBatch);
                          //  Joystick.Draw(Main.spriteBatch);


                            //


                        }
                        return true;












                    },
                    InterfaceScaleType.UI));
            }
        }

    }
}