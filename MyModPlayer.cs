using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.GameContent.RGB;
using Terraria.GameInput;
using FNATouch;
using UnlimitedZoom;

public class MyModPlayer : ModPlayer
{

    public static bool isLeft = false;
    public static bool isRight = false;
    public static bool isJump = false;
    public static bool isDown = false;
    public static bool isMouse = false;
    public static bool isUp = false;
    public static bool isSpace = false;
    public static bool isZoomIn = false;
    public VirtualGamepad virtualGamepad = new VirtualGamepad();
    public static bool 坐骑 = false;
    public static bool 自动模式 = false;
    public static bool 治疗药水 = false;
    public static bool 钩爪 = false;
    public static bool 火把 = false;
    public static bool isZoomOut = false;
    public static bool 使用物品 = false;
    public static bool 全部药水 = false;
    public static bool 魔法药水 = false;
    public override void ProcessTriggers(TriggersSet triggers)

    {

        Main.UIScale = 2.5f; // 设置为默认界面大小

        // Main.UIScale = ((float)UnlimitedZoomConfig.Instance.UIZoom) / 1f;

        Player p = Main.LocalPlayer;

        if (isLeft)
        {
            p.direction = -1; // 朝向左
            p.controlLeft = true;
        }
        if (isRight)
        {
            p.direction = 1; // 朝向右
            p.controlRight = true;
        }
        if (isDown)
        {
            p.controlDown = true;
        }
        if (isUp)
        {
            p.controlUp = true;
        }

        if (isJump)
        {
            p.controlUp = true;
            p.controlJump = true;
        }

        if (isZoomIn)
        {

          
        }
        if (isZoomOut)
        {
            //p.controlMount = true;
        }
        if (坐骑)
        {
            p.controlMount = true;
        }
        if (自动模式)
        {
            p.controlSmart = true;
        }
        if (治疗药水)
        {
            p.controlQuickHeal = true;
        }
        if (钩爪)
        {
            p.controlHook = true;
        }
        if (火把)
        {
            p.controlTorch = true;
        }
        if (全部药水)
        {
           

        }
        if (使用物品)
        {
            p.controlUseItem = true;

        }
        if (魔法药水)
        {
            p.controlQuickMana = true;
        }
    }

   

}
