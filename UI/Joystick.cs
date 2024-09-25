using DigitalHijinks.MiKeys.Helpers;
using FNATouch;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using Terraria;
using Terraria.ModLoader;

public static class Joystick
{
    private static Texture2D joystickBgTexture, joystickHandleTexture;
    private static Vector2 leftJoystickPosition, rightJoystickPosition;
    private static float joystickRadius = 80f; // 以背景纹理的一半为半径
    private static bool isLeftDragging = false, isRightDragging = false;
    private static Vector2 leftJoystickHandlePosition, rightJoystickHandlePosition;
    private static Vector2 leftDirection, rightDirection;
    private static int? leftJoystickTouchId = null;
private static int? rightJoystickTouchId = null;

    public static void LoadContent()
    {
        
    }

    public static void Update()
    {


        MyModPlayer.使用物品 = false;  // 确保默认状态为不攻击
        float uiScale = Main.UIScale;
        TouchCollection touchCollection = TouchPanel.GetState();
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X / uiScale, mouseState.Y / uiScale);
        Player p = Main.LocalPlayer;
        p.controlUseItem = false;  // 确保默认状态为不攻击
        //  bool isTouchingJoystick = false;

        foreach (TouchLocation touch in touchCollection)
        {
            Vector2 touchPosition = new Vector2(touch.Position.X / uiScale, touch.Position.Y / uiScale);

            // 检查触摸是否在左右摇杆的控制区域
            if ((touchPosition - leftJoystickPosition).Length() > joystickRadius &&
                (touchPosition - rightJoystickPosition).Length() > joystickRadius)
            {
                // 如果不是，则处理为非UI触摸
                HandleNonUITouch(touch, touchPosition, p);
            }

            // 处理摇杆交互
            HandleJoystickInteraction(ref isRightDragging, touchPosition, rightJoystickPosition, ref rightJoystickHandlePosition, ref rightDirection, ref rightJoystickTouchId, touch.State, touch.Id);
            HandleJoystickInteraction(ref isLeftDragging, touchPosition, leftJoystickPosition, ref leftJoystickHandlePosition, ref leftDirection, ref leftJoystickTouchId, touch.State, touch.Id);
        }
         if(ReLogic.OS.Platform.IsWindows)
  {
            HandleJoystickInteractionMouse(ref isRightDragging, mouseState.RightButton, mousePosition, rightJoystickPosition, ref rightJoystickHandlePosition, ref rightDirection);
            HandleJoystickInteractionMouse(ref isLeftDragging, mouseState.LeftButton, mousePosition, leftJoystickPosition, ref leftJoystickHandlePosition, ref leftDirection);
            



        }


    }
    private static void HandleNonUITouch(TouchLocation touch, Vector2 touchPosition, Player player)
    {
        if (touch.State == TouchLocationState.Pressed)
        {
            // 开始攻击
            player.controlUseItem = true;
        }
        else if (touch.State == TouchLocationState.Released)
        {
            // 停止攻击
            player.controlUseItem = false;
        }
    }


    private static void HandleJoystickInteraction(ref bool isDragging, Vector2 touchPosition, Vector2 joystickPosition, ref Vector2 joystickHandlePosition, ref Vector2 direction, ref int? joystickTouchId, TouchLocationState touchState, int touchId)
    {
        if (touchState == TouchLocationState.Pressed && (touchPosition - joystickPosition).Length() <= joystickRadius)
        {
            if (!joystickTouchId.HasValue)
            {
                joystickTouchId = touchId;
                isDragging = true;
            }
        }
        else if (touchState == TouchLocationState.Moved && joystickTouchId == touchId)
        {
            Vector2 directionVector = touchPosition - joystickPosition;
            float distance = directionVector.Length();
            if (distance > joystickRadius)
            {
                directionVector.Normalize();
                directionVector *= joystickRadius;
            }
            joystickHandlePosition = joystickPosition + directionVector;
            direction = directionVector / joystickRadius;
        }
        else if (touchState == TouchLocationState.Released && joystickTouchId == touchId)
        {
            isDragging = false;
            joystickHandlePosition = joystickPosition;
            direction = Vector2.Zero;
            joystickTouchId = null;
        }



        UpdatePlayerMovement(isDragging, direction);
    }

    private static void HandleJoystickInteractionMouse(ref bool isDragging, ButtonState buttonState, Vector2 Position, Vector2 joystickPosition, ref Vector2 joystickHandlePosition, ref Vector2 direction)
    {

        if (buttonState == ButtonState.Pressed && (Position - joystickPosition).Length() <= joystickRadius)
        {
            isDragging = true;


        }
        else if (buttonState == ButtonState.Released)
        {

            isDragging = false; // 这里重置拖动状态
        }

        if (isDragging)
        {
            Vector2 directionVector = Position - joystickPosition;
            float distance = directionVector.Length();
            if (distance > joystickRadius)
            {
                directionVector.Normalize();
                directionVector *= joystickRadius;
            }
            joystickHandlePosition = joystickPosition + directionVector;
            direction = directionVector / joystickRadius; // 使用缩放半径计算方向
        }
        else
        {
            joystickHandlePosition = joystickPosition; // 摇杆手柄回到中心位置
            direction = Vector2.Zero; // 方向归零
        }

        UpdatePlayerMovement(isDragging, direction);


    }

        private static Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, int radius, Color color)
    {
        int diameter = radius * 2;
        Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
        Color[] colorData = new Color[diameter * diameter];

        for (int x = 0; x < diameter; x++)
        {
            for (int y = 0; y < diameter; y++)
            {
                int distanceSquared = (x - radius) * (x - radius) + (y - radius) * (y - radius);
                if (distanceSquared < radius * radius)
                    colorData[x + y * diameter] = color;
                else
                    colorData[x + y * diameter] = Color.Transparent;
            }
        }

        texture.SetData(colorData);
        return texture;
    }


   

    private static void UpdatePlayerMovement(bool isDragging, Vector2 direction)
    {
        MyModPlayer.isJump = isDragging && direction.Y < -0.5f; // 向上跳跃
        MyModPlayer.isDown = isDragging && direction.Y > 0.5f; // 向下移动
        MyModPlayer.isLeft = isDragging && direction.X < -0.5f; // 向左移动
        MyModPlayer.isRight = isDragging && direction.X > 0.5f; // 向右移动
    }

   


    public static void Draw(SpriteBatch spriteBatch)
    {
         joystickBgTexture = CreateCircleTexture(Main.instance.GraphicsDevice, (int)joystickRadius, new Color(11, 19, 35, 80));
        joystickHandleTexture = CreateCircleTexture(Main.instance.GraphicsDevice, (int)(joystickRadius * 0.4f), new Color(180, 180, 180)); // 小一些的摇杆手柄

        // 其他初始化代码...
        int screenWidth = Main.screenWidth;
        int screenHeight = Main.screenHeight;
        float adjustedTop = screenHeight * 0.721f;
        float adjustedLeft = screenWidth * 0.198f;
       

        leftJoystickPosition = new Vector2(adjustedLeft, adjustedTop);
        rightJoystickPosition = new Vector2(screenWidth - adjustedLeft, adjustedTop);


        DrawJoystick(spriteBatch, leftJoystickPosition, leftJoystickHandlePosition);
        DrawJoystick(spriteBatch, rightJoystickPosition, rightJoystickHandlePosition);


    }

    private static void DrawJoystick(SpriteBatch spriteBatch, Vector2 joystickPosition, Vector2 joystickHandlePosition)
    {
        // 绘制摇杆背景
        spriteBatch.Draw(joystickBgTexture, joystickPosition - new Vector2(joystickRadius, joystickRadius), Color.White);
        spriteBatch.Draw(joystickHandleTexture, joystickHandlePosition - new Vector2(joystickHandleTexture.Width / 2, joystickHandleTexture.Height / 2), Color.White);

    }

    // 辅助方法用于绘制圆环

    private static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, int radius, int thickness, Color color)
    {
        Texture2D circleTexture = new Texture2D(spriteBatch.GraphicsDevice, 2 * radius, 2 * radius);
        Color[] colorData = new Color[2 * radius * 2 * radius];

        int innerRadiusSquared = (radius - thickness) * (radius - thickness);
        int outerRadiusSquared = radius * radius;

        for (int x = 0; x < 2 * radius; x++)
        {
            for (int y = 0; y < 2 * radius; y++)
            {
                int squareDist = (x - radius) * (x - radius) + (y - radius) * (y - radius);
                if (squareDist < outerRadiusSquared && squareDist >= innerRadiusSquared)
                    colorData[x + y * 2 * radius] = color;
                else
                    colorData[x + y * 2 * radius] = Color.Transparent;
            }
        }

        circleTexture.SetData(colorData);
        Main.spriteBatch.Draw(circleTexture, new Vector2(center.X - radius, center.Y - radius), Color.White);

    }




}
