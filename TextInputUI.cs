using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using System;

public class TextInputUI : UIState
{
    private UITextBox _textBox;
    private UIElement _area;
    private Action<string> _onTextSubmitted;

    public override void OnInitialize()
    {
        _area = new UIElement();
        _area.Width.Set(0, 0.8f); // 使用屏幕宽度的80%
        _area.Height.Set(60, 0f);
        _area.Top.Set(200, 0f); // 从屏幕顶部200像素处开始
        _area.HAlign = 0.5f; // 水平居中

        _textBox = new UITextBox("点击此处输入...");
        _textBox.Width.Set(0, 1f);
        _textBox.Height.Set(30, 0f);
        _textBox.Top.Set(15, 0f);
       //666 _textBox.OnTextSubmitted += TextBox_OnTextSubmitted;

        _area.Append(_textBox);
        Append(_area);
    }

    private void TextBox_OnTextSubmitted(UITextBox sender, EventArgs e)
    {
        _onTextSubmitted?.Invoke(sender.Text.Trim());
        _textBox.SetText(""); // 清空文本框
    }

    public void SetOnTextSubmitted(Action<string> onTextSubmitted)
    {
        _onTextSubmitted = onTextSubmitted;
    }
}
