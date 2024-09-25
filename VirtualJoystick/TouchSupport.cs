using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Terraria.GameContent;
using Terraria.Initializers;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;

namespace Terraria
{
    public static class TouchSupport
    {
        public static bool UseMouseFreeMode; // 是否使用鼠标自由模式

        public static TouchCollection touchSupport; // 支持的触摸集合

        public static TouchLocation[] touchLocations; // 存储触摸位置的数组

        public static Texture2D stickTexture; // 左侧摇杆的纹理

        public static Texture2D dotTexture; // 点的纹理，用于显示触摸点

        public static Vector2 LeftStick; // 左侧摇杆的位置

        public static Vector2 RightStick; // 右侧摇杆的位置

        private static PowerUIButton InventoryButton; // 库存按钮

        private static PowerUIButton SwapModeButton; // 切换模式按钮

        private static PowerUIButton EditButton; // 编辑按钮

        public static PowerUIButton AddButton; // 添加按钮

        private static PowerUIButton RemoveButton; // 移除按钮

        private static PowerUIButton ResetButton; // 重置按钮

        private static PowerUIButton ScaleUpButton; // 放大按钮

        private static PowerUIButton ScaleDownButton; // 缩小按钮

        public static float fadeLeft = 1f; // 左侧淡出效果

        public static float fadeRight = 1f; // 右侧淡出效果

        public static int SwapMouseMode = 1; // 切换鼠标模式的状态

        public static Vector2 pos1; // 用于存储一个位置的向量

        public static Vector2 pos2; // 用于存储另一个位置的向量

        public static bool LeftPressed; // 判断左侧是否被按下

        public static bool RightPressed; // 判断右侧是否被按下

        private static string[] modeText = new string[3] { "Free", "Stick", "Mixed" }; // 模式文本数组

        private static float[] modeTextPos = new float[3] { -3.25f, -1f, 0.5f }; // 模式文本的位置数组

        private static int timer; // 用于计时的变量

        private static float sinScale; // 正弦缩放的变量

        private static int doubleClickTimer; // 双击计时器

        private static int doubleClickCount; // 双击计数

        public static bool autoMode; // 自动模式的状态


        public static void LoadTextures()
        {
            stickTexture =ModContent.Request<Texture2D>("VirtualJoystick/JoyStick").Value;
            dotTexture = ModContent.Request < Texture2D >("VirtualJoystick/Dot").Value;
        }

        public static void InitializeUI()
        {
            InventoryButton = new PowerUIButton();
            InventoryButton.MySize = new Vector2(32f, 32f);
            InventoryButton.MyCenterType = CenterType.TopRight;
            SwapModeButton = new PowerUIButton();
            SwapModeButton.MySize = new Vector2(32f, 32f);
            SwapModeButton.MyCenterType = CenterType.TopRight;
            EditButton = new PowerUIButton();
            EditButton.MySize = new Vector2(32f, 32f);
            EditButton.MyCenterType = CenterType.TopRight;
            RemoveButton = new PowerUIButton();
            RemoveButton.MySize = new Vector2(32f, 32f);
            RemoveButton.MyCenterType = CenterType.TopRight;
            AddButton = new PowerUIButton();
            AddButton.MySize = new Vector2(32f, 32f);
            AddButton.MyCenterType = CenterType.TopRight;
            ResetButton = new PowerUIButton();
            ResetButton.MySize = new Vector2(32f, 32f);
            ResetButton.MyCenterType = CenterType.TopRight;
            ScaleUpButton = new PowerUIButton();
            ScaleUpButton.MySize = new Vector2(32f, 32f);
            ScaleUpButton.MyCenterType = CenterType.TopRight;
            ScaleDownButton = new PowerUIButton();
            ScaleDownButton.MySize = new Vector2(32f, 32f);
            ScaleDownButton.MyCenterType = CenterType.TopRight;
            PowerUIButton inventoryButton = InventoryButton;
            inventoryButton.MouseClickMe = (Action)Delegate.Combine(inventoryButton.MouseClickMe, (Action)delegate
            {
                Main.playerInventory = !Main.playerInventory;
            });
            PowerUIButton swapModeButton = SwapModeButton;
            swapModeButton.MouseClickMe = (Action)Delegate.Combine(swapModeButton.MouseClickMe, (Action)delegate
            {
                SwapMouseMode++;
                if (SwapMouseMode == 3)
                {
                    SwapMouseMode = 0;
                }
            });
            PowerUIButton editButton = EditButton;
            editButton.MouseClickMe = (Action)Delegate.Combine(editButton.MouseClickMe, (Action)delegate
            {
                TouchConfiguation.Editing = !TouchConfiguation.Editing;
                Main.playerInventory = false;
                if (!TouchConfiguation.Editing)
                {
                    TouchConfiguation.Save();
                }
            });
            PowerUIButton removeButton = RemoveButton;
            removeButton.MouseClickMe = (Action)Delegate.Combine(removeButton.MouseClickMe, (Action)delegate
            {
                TouchConfiguation.NeedToRemove = true;
            });
            PowerUIButton addButton = AddButton;
            addButton.MouseClickMe += (Action)delegate
            {
                string t = "";
                bool keyboard = false;

                if (!keyboard)
                {
                    keyboard = true;

                    // Get the user input
                 //   t = GetKeyboardInput();

                    // Add the hotkey button
                    TouchConfiguation.hotKeyButtons.Add(new HotKeyButton(new Vector2(40f, 40f), t)
                    {
                        MyRelativePos = new Vector2(Main.screenWidth / 2 + new Random().Next(-100, 101), Main.screenHeight / 2 + new Random().Next(-100, 101)) / Main.UIScale
                    });

                    // Reset keyboard flag if necessary
                    keyboard = false;
                }
            };
            PowerUIButton resetButton = ResetButton;
            resetButton.MouseClickMe = (Action)Delegate.Combine(resetButton.MouseClickMe, (Action)delegate
            {
                TouchConfiguation.Initialize();
            });
            PowerUIButton scaleUpButton = ScaleUpButton;
            scaleUpButton.MouseClickMe = (Action)Delegate.Combine(scaleUpButton.MouseClickMe, (Action)delegate
            {
                TouchConfiguation.NeedToScaleUp = true;
            });
            PowerUIButton scaleDownButton = ScaleDownButton;
            scaleDownButton.MouseClickMe = (Action)Delegate.Combine(scaleDownButton.MouseClickMe, (Action)delegate
            {
                TouchConfiguation.NeedToScaleDown = true;
            });
            PowerUIText powerUIText = new PowerUIText();
            powerUIText.MyScale = 0.5f;
            powerUIText.MyText = "...";
            powerUIText.MyCenterType = CenterType.TopRight;
            powerUIText.MyRelativePos = new Vector2(-7f, -4f);
            powerUIText.CanFocusMe = false;
            InventoryButton.Append(powerUIText);
            PowerUIText powerUIText2 = new PowerUIText();
            powerUIText2.MyScale = 0.3f;
            powerUIText2.MyText = "Mode";
            powerUIText2.MyCenterType = CenterType.TopRight;
            powerUIText2.MyRelativePos = new Vector2(0f, 8f);
            powerUIText2.CanFocusMe = false;
            SwapModeButton.Append(powerUIText2);
            PowerUIText powerUIText3 = new PowerUIText();
            powerUIText3.MyScale = 0.3f;
            powerUIText3.MyText = "Edit";
            powerUIText3.MyCenterType = CenterType.TopRight;
            powerUIText3.MyRelativePos = new Vector2(-3.5f, 8f);
            powerUIText3.CanFocusMe = false;
            EditButton.Append(powerUIText3);
            PowerUIText powerUIText4 = new PowerUIText();
            powerUIText4.MyScale = 0.25f;
            powerUIText4.MyText = "Remove";
            powerUIText4.MyCenterType = CenterType.TopRight;
            powerUIText4.MyRelativePos = new Vector2(1f, 8f);
            powerUIText4.CanFocusMe = false;
            RemoveButton.Append(powerUIText4);
            PowerUIText powerUIText5 = new PowerUIText();
            powerUIText5.MyScale = 0.33f;
            powerUIText5.MyText = "Add";
            powerUIText5.MyCenterType = CenterType.TopRight;
            powerUIText5.MyRelativePos = new Vector2(-5.4f, 8f);
            powerUIText5.CanFocusMe = false;
            AddButton.Append(powerUIText5);
            PowerUIText powerUIText6 = new PowerUIText();
            powerUIText6.MyScale = 0.28f;
            powerUIText6.MyText = "Reset";
            powerUIText6.MyCenterType = CenterType.TopRight;
            powerUIText6.MyRelativePos = new Vector2(-3.1f, 8f);
            powerUIText6.CanFocusMe = false;
            ResetButton.Append(powerUIText6);
            PowerUIText powerUIText7 = new PowerUIText();
            powerUIText7.MyScale = 0.28f;
            powerUIText7.MyText = "Size+";
            powerUIText7.MyCenterType = CenterType.TopRight;
            powerUIText7.MyRelativePos = new Vector2(-2.6f, 8f);
            powerUIText7.CanFocusMe = false;
            ScaleUpButton.Append(powerUIText7);
            PowerUIText powerUIText8 = new PowerUIText();
            powerUIText8.MyScale = 0.28f;
            powerUIText8.MyText = "Size-";
            powerUIText8.MyCenterType = CenterType.TopRight;
            powerUIText8.MyRelativePos = new Vector2(-2.6f, 8f);
            powerUIText8.CanFocusMe = false;
            ScaleDownButton.Append(powerUIText8);
            TouchConfiguation.Initialize();
            doubleClickCount = 0;
            doubleClickTimer = 0;
            autoMode = false;








            if (!TouchConfiguation.Editing)
            {
                int num = ((Main.mapStyle == 1) ? 260 : 0);
                InventoryButton.MyRelativePos = new Vector2(Main.screenWidth - 60 - num, 88f);
                SwapModeButton.MyRelativePos = new Vector2(Main.screenWidth - 60 - num, 128f);
                ((PowerUIText)SwapModeButton.children[0]).MyText = modeText[SwapMouseMode];
                ((PowerUIText)SwapModeButton.children[0]).MyRelativePos = new Vector2(modeTextPos[SwapMouseMode], 8f);
                InventoryButton.Active();
                SwapModeButton.Active();
            }
            if (Main.playerInventory || TouchConfiguation.Editing)
            {
                if (TouchConfiguation.Editing)
                {
                    RemoveButton.MyRelativePos = new Vector2(Main.screenWidth - 100, 48f);
                    AddButton.MyRelativePos = new Vector2(Main.screenWidth - 140, 48f);
                    ResetButton.MyRelativePos = new Vector2(Main.screenWidth - 60, 8f);
                    ScaleUpButton.MyRelativePos = new Vector2(Main.screenWidth - 180, 48f);
                    ScaleDownButton.MyRelativePos = new Vector2(Main.screenWidth - 220, 48f);
                    RemoveButton.Active();
                    AddButton.Active();
                    ResetButton.Active();
                    ScaleUpButton.Active();
                    ScaleDownButton.Active();
                    EditButton.MyColor = new Color(255, 230, 50);
                }
                else
                {
                    EditButton.MyColor = new Color(63, 82, 151) * 0.7f;
                }
                EditButton.MyRelativePos = new Vector2(Main.screenWidth - 60, 48f);
                ((PowerUIText)EditButton.children[0]).MyText = (TouchConfiguation.Editing ? "Exit" : "Edit");
                EditButton.Active();
            }
            if (!Main.playerInventory)
            {
                for (int i = 0; i < TouchConfiguation.hotKeyButtons.Count; i++)
                {
                    TouchConfiguation.hotKeyButtons[i].Active();
                }
            }
        }

        public static void ActiveButtons()
        {

        }

        public static void DrawJoyStick(int offset = 0)
        {
            InitializeUI();
            LoadTextures();
            ActiveButtons();
            if (!TouchConfiguation.Editing)
            {
                InventoryButton.Draw(Main.spriteBatch);
                SwapModeButton.Draw(Main.spriteBatch);
            }

            if (Main.playerInventory || TouchConfiguation.Editing)
            {
                if (TouchConfiguation.Editing)
                {
                    RemoveButton.Draw(Main.spriteBatch);
                    AddButton.Draw(Main.spriteBatch);
                    ResetButton.Draw(Main.spriteBatch);
                    ScaleUpButton.Draw(Main.spriteBatch);
                    ScaleDownButton.Draw(Main.spriteBatch);
                }
                EditButton.Draw(Main.spriteBatch);
            }
            if (!Main.playerInventory)
            {
                for (int i = 0; i < TouchConfiguation.hotKeyButtons.Count; i++)
                {
                    TouchConfiguation.hotKeyButtons[i].Draw(Main.spriteBatch);
                }
            }
            fadeLeft *= 1f;
            fadeRight *= 1f;
            if (timer != int.MaxValue)
            {
                timer++;
            }
            else
            {
                timer = 0;
            }
            sinScale = (float)Math.Sin((float)timer / 1.571f) / 6f + 1f;
            if (!UseMouseFreeMode || (Main.playerInventory && SwapMouseMode != 0))
            {
                Main.spriteBatch.Draw(TextureAssets.Cursors[13].Value, new Vector2(-22f, -22f) * sinScale / Main.UIScale + ((SwapMouseMode == 1) ? ((Main.LocalPlayer.MountedCenter - Main.screenPosition) / Main.UIScale + RightStick * 2f) : Main.MouseScreen), new Rectangle(0, 0, 22, 22), Color.White * ((SwapMouseMode == 1) ? fadeRight : 1f), 0f, Vector2.Zero, sinScale, SpriteEffects.None, 0f);
                LeftPressed = false;
                RightPressed = false;
                LeftStick = Helper.Interpolation(LeftStick, Vector2.Zero, 1f, 4f);
                RightStick = Helper.Interpolation(RightStick, Vector2.Zero, 1f, 4f);
                if (LeftStick.Length() < 2f)
                {
                    LeftStick = Vector2.Zero;
                }
                if (RightStick.Length() < 2f)
                {
                    RightStick = Vector2.Zero;
                }
                Vector2 vector = TouchConfiguation.LeftStickSize * 0.66f;
                pos1 = new Vector2(60 + offset, (float)(Main.screenHeight - 50) - vector.Y * 2f);
                Main.spriteBatch.Draw(stickTexture, pos1, new Rectangle(0, 0, 127, 127), Color.White * 0.4f * fadeLeft, 0f, Vector2.Zero, 1.33f * TouchConfiguation.LeftStickScale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(stickTexture, pos1, new Rectangle(254, 0, 127, 127), Color.White * 0.1f * fadeLeft, 0f, Vector2.Zero, 1.33f * TouchConfiguation.LeftStickScale, SpriteEffects.None, 0f);
                pos1 += vector;
                vector = TouchConfiguation.RightStickSize * 0.66f;
                pos2 = new Vector2((float)(Main.screenWidth - 60) - vector.X - (float)offset, (float)(Main.screenHeight - 50) - vector.Y) - vector;
                Main.spriteBatch.Draw(stickTexture, pos2, new Rectangle(127, 0, 127, 127), Color.White * 0.4f * fadeRight, 0f, Vector2.Zero, 1.33f * TouchConfiguation.RightStickScale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(stickTexture, pos2, new Rectangle(254, 0, 127, 127), Color.White * 0.1f * fadeRight, 0f, Vector2.Zero, 1.33f * TouchConfiguation.RightStickScale, SpriteEffects.None, 0f);
                pos2 += vector;
                CheckCollision();
                vector = new Vector2(34f, 34f) * 1.33f * TouchConfiguation.LeftStickScale;
                Main.spriteBatch.Draw(dotTexture, pos1 - vector / 2f + LeftStick, new Rectangle(0, 0, 34, 34), Color.White * fadeLeft, 0f, Vector2.Zero, 1.33f * TouchConfiguation.LeftStickScale, SpriteEffects.None, 0f);
                vector = new Vector2(34f, 34f) * 1.33f * TouchConfiguation.RightStickScale;
                Main.spriteBatch.Draw(dotTexture, pos2 - vector / 2f + RightStick, new Rectangle(autoMode ? 68 : 34, 0, 34, 34), Color.White * fadeRight, 0f, Vector2.Zero, 1.33f * TouchConfiguation.RightStickScale, SpriteEffects.None, 0f);
            }
        }

        public static void CheckCollision()
        {
            float num = 127f + fadeLeft * 123f;
            float num2 = 127f + fadeRight * 123f;
            if (doubleClickTimer > 0)
            {
                doubleClickTimer--;
            }
            if (doubleClickTimer == 0)
            {
                doubleClickCount = 0;
            }
            if (doubleClickCount >= 2)
            {
                doubleClickCount = 0;
                autoMode = !autoMode;
            }
            if (autoMode && !TouchConfiguation.Editing)
            {
                if ((Main.LocalPlayer.HeldItem.autoReuse || Main.LocalPlayer.HeldItem.channel))
                {
                    float num3 = new Vector2(Main.screenWidth, Main.screenHeight).Length() / 2f;
                    NPC nPC = null;
                    NPC[] npc = Main.npc;
                    foreach (NPC nPC2 in npc)
                    {
                        if (nPC2.CanBeChasedBy() && !nPC2.friendly && nPC2.Distance(Main.LocalPlayer.MountedCenter) < num3 * (float)((!nPC2.boss) ? 1 : 2))
                        {
                            nPC = nPC2;
                            num3 = nPC2.Distance(Main.LocalPlayer.MountedCenter);
                        }
                    }
                    if (nPC != null)
                    {
                        fadeRight = 1f;
                        RightPressed = true;
                        RightStick = Helper.ToUnitVector(nPC.Center - Main.LocalPlayer.MountedCenter) * 60f * TouchConfiguation.RightStickScale;
                    }
                }
                else
                {
                    autoMode = false;
                }
            }
            if (TouchConfiguation.Editing)
            {
                fadeLeft = 1f;
                fadeRight = 1f;
            }


            TouchCollection touchLocations  = TouchPanel.GetState();





            for (int j = 0; j < touchLocations.Count; j++)
            {
                if (TouchConfiguation.Editing)
                {
                    if (Vector2.Distance(new Vector2(60f, Main.screenHeight - 50), touchLocations[j].Position / Main.UIScale) < TouchConfiguation.LeftStickSize.X * 1.33f)
                    {
                        TouchConfiguation.LeftStickScale = Vector2.Distance(new Vector2(60f, Main.screenHeight - 50), touchLocations[j].Position / Main.UIScale) / 127f;
                        if (TouchConfiguation.LeftStickScale < 0.33f)
                        {
                            TouchConfiguation.LeftStickScale = 0.33f;
                        }
                    }
                    if (Vector2.Distance(new Vector2(Main.screenWidth - 60, Main.screenHeight - 50), touchLocations[j].Position / Main.UIScale) < TouchConfiguation.RightStickSize.X * 1.33f)
                    {
                        TouchConfiguation.RightStickScale = Vector2.Distance(new Vector2(Main.screenWidth - 60, Main.screenHeight - 50), touchLocations[j].Position / Main.UIScale) / 127f;
                        if (TouchConfiguation.RightStickScale < 0.33f)
                        {
                            TouchConfiguation.RightStickScale = 0.33f;
                        }
                    }
                    continue;
                }
                if (Vector2.Distance(pos2, touchLocations[j].Position / Main.UIScale) < num2 * TouchConfiguation.RightStickScale)
                {
                    RightStick = touchLocations[j].Position / Main.UIScale - pos2;
                    RightPressed = true;
                    if (RightStick.Length() > 84f * TouchConfiguation.RightStickScale)
                    {
                        RightStick = Helper.ToUnitVector(RightStick) * 84f * TouchConfiguation.RightStickScale;
                    }
                    if (RightStick != Vector2.Zero && RightStick.Length() < 40f * TouchConfiguation.RightStickScale && doubleClickTimer < 3 && doubleClickCount < 2)
                    {
                        RightStick = Vector2.Zero;
                        doubleClickTimer = 4;
                        doubleClickCount++;
                    }
                    doubleClickTimer++;
                    fadeRight = 1f;
                }
                if (Vector2.Distance(pos1, touchLocations[j].Position / Main.UIScale) < num * TouchConfiguation.LeftStickScale)
                {
                    LeftStick = touchLocations[j].Position / Main.UIScale - pos1;
                    LeftPressed = true;
                    if (LeftStick.Length() > 84f * TouchConfiguation.LeftStickScale)
                    {
                        LeftStick = Helper.ToUnitVector(LeftStick) * 84f * TouchConfiguation.LeftStickScale;
                    }
                    fadeLeft = 1f;
                }
            }
        }
    }
}
