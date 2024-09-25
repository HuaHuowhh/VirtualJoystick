using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using ProjectStarlight.Interchange;
using ProjectStarlight.Interchange.Utilities;
using ReLogic.Graphics;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace FNATouch
{
    public class VirtualGamepad
    {
        public class VirtualButton
        {

            internal Rectangle Bounds;  // 按钮的边界矩形
            internal bool Active = true;  // 按钮是否激活
            internal ButtonState PreviousState = ButtonState.Pressed;  // 按钮的前一个状态
            internal ButtonState State = ButtonState.Pressed;  // 当前按钮状态
            public bool IsBeingDragged = false;  // 按钮是否正在被拖动
            public string Label;  // 按钮标签
            public TextureGIF CustomGIF { get; set; } // 新增 GIF 属性
            public Action OnClick { get; set; }  // 点击按钮时的事件处理
            public Action OnRelease { get; set; } // 新增释放按钮时的行为
            public bool Visible = true;  // 控制按钮的可见性
            public Vector2 DragOffset { get; set; } // 拖动偏移
            public Color DrawColor { get;  set; }  // 绘制按钮的颜色
            public Texture2D CustomTexture { get;  set; }
        }
        static VirtualGamepad virtualGamepad = new VirtualGamepad();
        static KeyConfiguration keyConfiguration = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard];// 获取当前键盘配置

        static Dictionary<string, VirtualButton> buttons = new Dictionary<string, VirtualButton>();// 按钮字典
        private static Dictionary<string, bool> buttonStates = new Dictionary<string, bool>();
        static int screenHeight, screenWidth;
        public static bool text = false;// 添加一个标志位来表示是否处于文本模式
        private static bool isInEditMode = true;// 添加一个标志位来表示是否处于编辑模式
        private static bool isMenuOpen = false;  // 添加一个标志位来表示菜单是否打开
        public static int buttonRadius = Math.Min(buttonWidth, buttonHeight) / 2;
        public static int buttonWidth = (int)(60 * Main.UIScale * controlScale);
        public static int buttonHeight = (int)(60 * Main.UIScale * controlScale);

        private static DateTime lastToggleTime;// 添加一个变量来存储上次切换时间
        public static Texture2D buttonTexture;// 按钮纹理

        public static Texture2D 生命药水;// 按钮纹理
        public static Texture2D 魔法药水;// 按钮纹理
        public static Texture2D 背包;// 按钮纹理
        public static Texture2D 当前物品;// 按钮纹理
        private static bool isScalingMode = false; // 是否处于缩放模式
        private static Color currentColor;// = new Color(32, 32, 32); // RGB值对应325167

        public static float controlScale = 1.0f; // 初始缩放比例
                                                 //    private static bool isDraggingMode = false; // 是否启用拖动模式
                                                 //  private static Texture2D circleTexture; // 圆形纹理

        // static float uiScale = Main.UIScale;
        private static bool isDraggingMode = false; // 是否启用拖动模式

        public static void RemoveButton(string action)
        {

            buttons.Remove(action);// 移除按钮


        }
        public static void SetVisibility(string action, bool isVisible)
        {
            if (buttons.ContainsKey(action))// 检查按钮是否存在
            {
                buttons[action].Visible = isVisible;// 设置按钮的可见性
                if (!isVisible)
                {
                    // 如果我们正在隐藏按钮，确保也停止它的拖动
                    buttons[action].IsBeingDragged = false;// 停止按钮的拖动
                }
            }
            else
            {
                Main.NewText($"无法找到按钮: {action}");
            }
        }
        public static void SetActive(string action, bool active)
        {
            buttons[action].Active = active;// 设置按钮的激活状态
            if (!active)
            {
                // Reset
                buttons[action].PreviousState = ButtonState.Released;// 重置状态
                buttons[action].State = ButtonState.Released;// 重置状态
            }
        }
        public static void DisableAllButtons()
        {
            foreach (var button in buttons.Values)
            {

                button.Active = false; // 设置每个按钮为不激活状态
                button.PreviousState = ButtonState.Released; // 重置状态
                button.State = ButtonState.Released; // 重置状态
            }
        }

        public static void AddButton(string action, float xFraction, float yFraction, int width, int height, string label, Action onPress, Action onRelease)
        {
            int screenWidth = Main.screenWidth;
            int screenHeight = Main.screenHeight;

            int x = (int)(xFraction * screenWidth);
            int y = (int)(yFraction * screenHeight); // 计算按钮位置
            if (!keyConfiguration.KeyStatus.ContainsKey(action))
            {
                keyConfiguration.KeyStatus[action] = new List<string>(); // 确保按键动作存在于配置中
            }

            if (!buttons.ContainsKey(action))
            {
                buttons[action] = new VirtualButton { Bounds = new Rectangle(x, y, width, height), Label = label, OnClick = onPress, OnRelease = onRelease };
            }
            else
            {

                buttons[action].Bounds = new Rectangle(x, y, width, height);// 设置按钮的边界
                buttons[action].Label = label;// 设置按钮的标签
                buttons[action].OnClick = onPress;// 设置按钮的点击事件
                buttons[action].OnRelease = onRelease;// 设置按钮的点击和释放事件
            }
        }


       
        public static void InitializeGraphics()
        {




           
            if (isInEditMode)
            {
                //    SetVisibility("退出编辑", false);
                //   SetVisibility("放大按钮", false);
                //   SetVisibility("缩小按钮", false);


               

            }

          

            生命药水 = ModContent.Request<Texture2D>("VirtualJoystick/生命药水").Value;
            魔法药水 = ModContent.Request<Texture2D>("VirtualJoystick/魔法药水").Value;
            背包 = ModContent.Request<Texture2D>("VirtualJoystick/背包").Value;
            Player player = Main.LocalPlayer;

            // 获取当前使用的物品
            Item currentItem = player.HeldItem;
            当前物品  =  TextureAssets.Item[currentItem.type].Value;

            buttonTexture = ModContent.Request<Texture2D>("VirtualJoystick/控件").Value;


           

            //circleTexture = ModContent.Request<Texture2D>("VirtualJoystick/JoystickKnob").Value; // 加载圆形纹理
            // 加载按钮纹理

            // 添加方向控制按钮
            AddDirectionalButtons();

            GamePadState realGamePadState = GamePad.GetState(PlayerIndex.One);
            TouchCollection touches = TouchPanel.GetState();
            MouseState mouseState = Mouse.GetState();

            HandleTouchInput(touches); // 处理触屏输入
           HandleMouseInput(mouseState); // 处理鼠标输入
        }

        // 添加方向控制按钮
        private static void AddDirectionalButtons()
        {
            /* AddButton("W", 0.15f, 0.6f, buttonWidth, buttonHeight, "W",

                 () => { MyModPlayer.isUp = true; },
                 () => { MyModPlayer.isUp = false; });
             AddButton("A", 0.10f, 0.7f, buttonWidth, buttonHeight, "A",
                 () => { MyModPlayer.isLeft = true; },
                 () => { MyModPlayer.isLeft = false; });
             AddButton("S", 0.15f, 0.7f, buttonWidth, buttonHeight, "S",
                 () => { MyModPlayer.isDown = true; },
                 () => { MyModPlayer.isDown = false; });
             AddButton("D", 0.20f, 0.7f, buttonWidth, buttonHeight, "D",
                 () => { MyModPlayer.isRight = true; },
                 () => { MyModPlayer.isRight = false; });

             AddButton("Space", 0.80f, 0.7f, buttonWidth, buttonHeight, "Space",
               () => { MyModPlayer.isJump = true; },
               () => { MyModPlayer.isJump = false; });
             AddButton("Map", 0.85f, 0.7f, buttonWidth, buttonHeight, "Map",
                   () => { MyModPlayer.isMap = true; },
                 () => { MyModPlayer.isMap = false; });
             */
         
            AddButton1("ESC", 0.5f, 0.1f, 50, 50, "ESC", async () =>
            {
                if (!isMenuOpen)
                {
                    // 打开菜单的逻辑
                    isMenuOpen = true;
                    Main.playerInventory = true; // 更新菜单状态为打开
                                                 // 这里可以添加其他打开菜单的操作
                }
                else
                {
                    // 关闭菜单的逻辑
                    isMenuOpen = false;
                    Main.playerInventory = false; // 更新菜单状态为关闭
                                                  // 这里可以添加其他关闭菜单的操作
                }
            });



            AddButton1("主题切换", 0.6f, 0.1f, 50, 50, "主题切换", async () =>
            {
                // 切换颜色逻辑
                if (currentColor == Color.White) // 当前是白色
                {
                    currentColor = new Color(135, 150, 155); // 切换到更亮的 #4B585C
                    Main.NewText("安静主题");
                }
                else if (currentColor == new Color(135, 150, 155)) // 当前是亮版 #4B585C
                {
                    currentColor = new Color(0, 100, 140); // 切换到更亮的 #004565
                    Main.NewText("炫酷主题");
                }
                else
                {
                    currentColor = Color.White; // 切换回白色
                    Main.NewText("默认主题");  // 显示消息
                }
            });
            /*  AddButton1("编辑模式", 0.7f, 0.1f, 50, 50, "编辑模式", () =>
              {

                  isInEditMode = false;

                  VirtualGamepad.SetVisibility("退出编辑", true);  // 隐藏 W 按钮
                  VirtualGamepad.SetVisibility("放大按钮", true);  // 隐藏 W 按钮
                  VirtualGamepad.SetVisibility("缩小按钮", true);  // 隐藏 W 按钮
                  VirtualGamepad.SetVisibility("拖动模式", true);  // 隐藏 W 按钮
                                                               // Main.NewText("编辑模式");  // 显示消息，实际的打开键盘逻辑根据你的需求添加
              });
              AddButton1("拖动模式", 0.65f, 0.1f, 50, 50, "拖动模式", () =>
              {
                  isScalingMode = false; // 退出缩放模式
                  isDraggingMode = !isDraggingMode; // 切换拖动模式
                  Main.NewText(isDraggingMode ? "拖动模式已开启" : "拖动模式已关闭");
              });


              AddButton1("退出编辑", 0.75f, 0.1f, 50, 50, "退出编辑", () =>
              {

                  isInEditMode = true;

                  text = false;

                  Main.NewText("退出");  // 显示消息，实际的打开键盘逻辑根据你的需求添加
              });
              AddButton1("放大按钮", 0.80f, 0.1f, 50, 50, "放大按钮", () =>
              {
                  isScalingMode = true; // 进入缩放模式
                  controlScale += 0.1f; // 增加缩放比例
              });

              AddButton1("缩小按钮", 0.60f, 0.2f, 50, 50, "缩小按钮", () =>
              {
                  float scale = 0.5f; // 0.5表示缩小为50%


                  isScalingMode = true; // 进入缩放模式
                  controlScale = Math.Max(controlScale - 0.1f, 0.5f); // 减少缩放比例
              });

              AddButton1("AddCircleButton", 0.60f, 0.3f, 50, 50, "缩小按钮", () =>
              {
                  float scale = 0.5f; // 0.5表示缩小为50%


                  //  isScalingMode = true; // 进入缩放模式
                  //  controlScale = Math.Max(controlScale - 0.1f, 0.5f); // 减少缩放比例
              });
              */
            AddButtonTexture("生命药水", 0.28f, 0.58f, 50, 50, "生命药水", 生命药水 ,() => { MyModPlayer.治疗药水 = true; },

                 () => { MyModPlayer.治疗药水 = false; });

            AddButtonTexture("背包", 0.08f, 0.78f, 50, 50, "背包", 背包, () =>
            {
                if (!isMenuOpen)
                {
                    // 打开菜单的逻辑
                    isMenuOpen = true;
                    Main.playerInventory = true; // 更新菜单状态为打开
                                                 // 这里可以添加其他打开菜单的操作
                }
                else
                {
                    // 关闭菜单的逻辑
                    isMenuOpen = false;
                    Main.playerInventory = false; // 更新菜单状态为关闭
                                                  // 这里可以添加其他关闭菜单的操作
                }
            });


            AddButton1("全部药水", 0.25f, 0.3f, 40, 40, "全部药水", () => {




                //PlayerInput.gamepadMode = !PlayerInput.gamepadMode; // 切换手柄





            });


            AddButtonTexture("当前物品", 0.08f, 0.58f, 30, 30, "当前物品",当前物品 , () => { MyModPlayer.使用物品 = true; },

                 () => { MyModPlayer.使用物品 = false; });
            AddButton1("药剂", 0.28f, 0.78f, 40, 40, "药剂", () =>
            {
               // SimulateKeyPress(SDL.SDL_Keycode.SDLK_d);

              //  var mouseSimulator = new MouseSimulator();
              //  mouseSimulator.RightButtonClick(); // 左键点击



                //  isScalingMode = true; // 进入缩放模式
                //  controlScale = Math.Max(controlScale - 0.1f, 0.5f); // 减少缩放比例
            });
            AddButton1("工具", 0.05f, 0.68f, 40, 40, "工具", () => {
                if (!isMenuOpen)
                {
                    // 打开菜单的逻辑
                    isMenuOpen = true;
                    MyModPlayer.火把 = true; // 更新菜单状态为打开
                                                 // 这里可以添加其他打开菜单的操作
                }
                else
                {
                    // 关闭菜单的逻辑
                    isMenuOpen = false;
                    MyModPlayer.火把 = false; // 更新菜单状态为关闭
                                                  // 这里可以添加其他关闭菜单的操作
                }
            });
            AddButtonTexture("魔法药水", 0.30f, 0.68f, 40, 40, "魔法药水", 魔法药水,() => { MyModPlayer.魔法药水 = true; },

                 () => { MyModPlayer.魔法药水 = false; });

            AddButton1("放大", 0.33f, 0.78f, 40, 40, "放大", () => { MyModPlayer.isZoomIn = true; },

                 () => { MyModPlayer.isZoomIn = false; });
            AddButton1("缩小", 0.68f, 0.78f, 40, 40, "缩小", () => { MyModPlayer.isZoomOut = true; },

                 () => { MyModPlayer.isZoomOut = false; });

            AddButton1("自动模式", 0.68f, 0.88f, 40, 40, "自动模式", () => { MyModPlayer.自动模式 = true; },

                 () => { MyModPlayer.自动模式 = false; });
        
    
        AddButton1("钩抓", 0.68f, 0.68f, 50, 50, "钩抓", () => { MyModPlayer.钩爪 = true; },

                 () => { MyModPlayer.钩爪 = false; });

            AddButton1("坐骑", 0.66f, 0.58f, 40, 40, "坐骑", () => { MyModPlayer.坐骑 = true; },

                 () => { MyModPlayer.坐骑 = false; });
        }

        public static void AddButton1(string action, float xFraction, float yFraction, int width, int height, string label, Action onClick, Action onSecondClick = null)
        {
            int screenWidth = Main.screenWidth;
            int screenHeight = Main.screenHeight;

            int x = (int)(xFraction * screenWidth);
            int y = (int)(yFraction * screenHeight); // 计算按钮位置

            // 初始化按钮状态
            if (!buttonStates.ContainsKey(action))
            {
                buttonStates[action] = false; // 默认为关闭状态
            }

            if (!keyConfiguration.KeyStatus.ContainsKey(action))
            {
                keyConfiguration.KeyStatus[action] = new List<string>(); // 初始化按键列表
            }

            if (!buttons.ContainsKey(action))
            {
                buttons[action] = new VirtualButton
                {
                    Bounds = new Rectangle(x, y, width, height),
                    Label = label,
                    OnClick = async () => await HandleButtonClick(action, onClick, onSecondClick)
                };
            }
            else
            {
                buttons[action].Bounds = new Rectangle(x, y, width, height);
                buttons[action].Label = label;
                buttons[action].OnClick = async () => await HandleButtonClick(action, onClick, onSecondClick);
            }
        }

        public static void AddButtonTexture(string action, float xFraction, float yFraction, int width, int height, string label, Texture2D customTexture , Action onClick, Action onSecondClick = null)
        {
            int screenWidth = Main.screenWidth;
            int screenHeight = Main.screenHeight;

            int x = (int)(xFraction * screenWidth);
            int y = (int)(yFraction * screenHeight); // 计算按钮位置

            // 初始化按钮状态
            if (!buttonStates.ContainsKey(action))
            {
                buttonStates[action] = false; // 默认为关闭状态
            }

            if (!keyConfiguration.KeyStatus.ContainsKey(action))
            {
                keyConfiguration.KeyStatus[action] = new List<string>(); // 初始化按键列表
            }

            if (!buttons.ContainsKey(action))
            {
                buttons[action] = new VirtualButton
                {
                    Bounds = new Rectangle(x, y, width, height),
                    Label = label,
                    CustomTexture = customTexture, // 设置自定义纹理
                    OnClick = async () => await HandleButtonClick(action, onClick, onSecondClick)
                };
            }
            else
            {
                buttons[action].Bounds = new Rectangle(x, y, width, height);
                buttons[action].Label = label;
                buttons[action].CustomTexture = customTexture; // 更新自定义纹理
                buttons[action].OnClick = async () => await HandleButtonClick(action, onClick, onSecondClick);
            }
        }

        private static async Task HandleButtonClick(string action, Action onClick, Action onSecondClick)
        {
            // 如果按钮已被处理，返回
            if (buttonStates[action]) return;

            // 设置状态为“正在处理”
            buttonStates[action] = true;

            // 调用点击事件
            onClick.Invoke(); // 第一次点击

            // 等待一段时间再恢复状态
            await Task.Delay(200); // 200毫秒延迟
            buttonStates[action] = false; // 恢复状态

            // 如果按钮状态仍然为关闭，调用第二次点击事件
            if (!buttonStates[action])
            {
                onSecondClick?.Invoke(); // 第二次点击
            }
        }



     



        private static void CheckButtonPress(Vector2 position, bool isPressed)
        {
            Vector2 scaledPosition = new Vector2(position.X / Main.UIScale, position.Y / Main.UIScale);

            foreach (var action in buttons.Keys.ToList())
            {
                VirtualButton button = buttons[action];

                if (button.Active && button.Bounds.Contains((int)scaledPosition.X, (int)scaledPosition.Y))
                {
                    if (isPressed)
                    {
                        if (!button.IsBeingDragged)
                        {
                            button.IsBeingDragged = true;
                            button.DragOffset = new Vector2(scaledPosition.X - button.Bounds.X, scaledPosition.Y - button.Bounds.Y);
                            button.OnClick?.Invoke(); // 确保只在开始拖动时调用
                        }
                    }
                    else
                    {
                        if (button.IsBeingDragged)
                        {
                            button.IsBeingDragged = false;
                            button.Bounds.X = (int)(scaledPosition.X - button.DragOffset.X);
                            button.Bounds.Y = (int)(scaledPosition.Y - button.DragOffset.Y);
                            button.OnRelease?.Invoke();
                        }
                    }
                }
                else if (button.IsBeingDragged)
                {
                    // 仅更新位置，如果需要，可以启用这部分
                    // button.Bounds.X = (int)(scaledPosition.X - button.DragOffset.X);
                    // button.Bounds.Y = (int)(scaledPosition.Y - button.DragOffset.Y);
                }
            }
        }





        public static void Draw(SpriteBatch spriteBatch)
        {
            DynamicSpriteFont MyFont = FontAssets.MouseText.Value;

            foreach (KeyValuePair<string, VirtualButton> kvp in buttons)
            {
                VirtualButton button = kvp.Value;

                if (!button.Visible) continue;

                Color currentColor = new Color(255, 255, 255, 255);
                Color drawColor = button.Active ? (button.IsBeingDragged ? currentColor * 0.7f : currentColor) : Color.Gray;

                // 如果有自定义 GIF，则绘制 GIF
                if (button.CustomGIF != null)
                {
                    //button.CustomGIF.UpdateGIF(); // 更新 GIF
                    button.CustomGIF.Draw(spriteBatch, button.Bounds.Location.ToVector2(), drawColor);
                    button.CustomGIF.Play(); // 取消注释，确保播放
                }

                else
                {
                    // 使用按钮的自定义纹理
                    Texture2D textureToDraw = button.CustomTexture ?? buttonTexture;
                    spriteBatch.Draw(textureToDraw, button.Bounds, drawColor);
                }

                // 绘制文本
                if (button.CustomTexture == null && button.CustomGIF == null)
                {
                    float textSizeUisale = 1 / controlScale;
                    Vector2 textSize = MyFont.MeasureString(kvp.Key) * textSizeUisale;

                    float maxWidth = button.Bounds.Width * 0.9f;
                    float maxHeight = button.Bounds.Height * 0.9f;

                    float xScale = maxWidth / textSize.X;
                    float yScale = maxHeight / textSize.Y;
                    float scale = Math.Min(Math.Min(xScale, yScale), 1f);

                    Vector2 textPosition = new Vector2(button.Bounds.Center.X - (textSize.X * scale) / 2,
                                                        button.Bounds.Center.Y - (textSize.Y * scale) / 2);

                    spriteBatch.DrawString(MyFont, kvp.Key, textPosition, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
        }



        private static void HandleTouchInput(TouchCollection touches)
        {
            foreach (var touch in touches)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    CheckButtonPress(touch.Position, true);
                }
                else if (touch.State == TouchLocationState.Released)
                {
                    CheckButtonPress(touch.Position, false);
                }
            }
        }

        // 处理鼠标输入
        private static void HandleMouseInput(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                CheckButtonPress(new Vector2(mouseState.X, mouseState.Y), true);
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                CheckButtonPress(new Vector2(mouseState.X, mouseState.Y), false);
            }
        }
        
    }

}
