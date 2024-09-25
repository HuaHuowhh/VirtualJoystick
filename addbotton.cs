using FNATouch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace LansUILib
{
    public class addbotton
    {
        public static void OpenKeyboard(string prompt, Action<string> onTextSubmitted)
        {
            TextInputUI inputUI = new TextInputUI();
            inputUI.SetOnTextSubmitted(onTextSubmitted);

            UserInterface ui = new UserInterface();
            ui.SetState(inputUI);
          //666  Main.instance.RegisterUI(ui);  // 假设你有一个方法来注册和管理UI

            Main.NewText(prompt);  // 可以在游戏的聊天窗口中显示提示
        }

        // 用户输入处理后添加按钮的逻辑
        public static void AddButtonWithUserInput()
        {
            // 弹出键盘让用户输入按钮标签
            OpenKeyboard("请输入按钮标签:", label =>
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    Main.NewText("按钮标签不能为空！");
                    return;
                }

                // 弹出键盘让用户输入X坐标
                OpenKeyboard("请输入按钮的X坐标 (0-1，例如0.5):", xText =>
                {
                    float xFraction;
                    if (!float.TryParse(xText, out xFraction) || xFraction < 0 || xFraction > 1)
                    {
                        Main.NewText("无效的X坐标！");
                        return;
                    }

                    // 弹出键盘让用户输入Y坐标
                    OpenKeyboard("请输入按钮的Y坐标 (0-1，例如0.5):", yText =>
                    {
                        float yFraction;
                        if (!float.TryParse(yText, out yFraction) || yFraction < 0 || yFraction > 1)
                        {
                            Main.NewText("无效的Y坐标！");
                            return;
                        }

                        // 添加按钮
                       

                        Main.NewText($"{label} 按钮已添加在 ({xFraction}, {yFraction})");
                    });
                });
            });
        }
    }
}
