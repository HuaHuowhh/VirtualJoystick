using System;
using System.Text;
using System.Threading.Tasks;
using LansUILib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

namespace Microsoft.Xna.Framework.Input
{
    public static class KeyboardInputFNA
    {
        private static TaskCompletionSource<string> _tcs;
        private static string _inputText = "";
        private static DynamicSpriteFont _font = FontAssets.MouseText.Value;
        private static Texture2D _backgroundTexture;

        public static Task<string> Show(DynamicSpriteFont font, string defaultText)
        {

            _tcs = new TaskCompletionSource<string>();
            _inputText = defaultText;
            _font = font;

            // Create a simple background texture
            _backgroundTexture = new Texture2D(Main.instance.GraphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { Color.White });

            // Run the input handling in a separate task
            Task.Run(() => InputLoop());

            return _tcs.Task;
        }

        private static void InputLoop()
        {
            while (!_tcs.Task.IsCompleted)
            {
                var keyboardState = Keyboard.GetState();

                // Handle text input
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (keyboardState.IsKeyDown(key))
                    {
                        // Handle text input and backspace
                        if (key == Keys.Enter)
                        {
                            _tcs.SetResult(_inputText);
                            break;
                        }
                        else if (key == Keys.Back && _inputText.Length > 0)
                        {
                            _inputText = _inputText.Substring(0, _inputText.Length - 1);
                        }
                    }
                }

                // Implement a way to display the input box
                // This should be part of your main update or draw loop
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
           // spriteBatch = Main.spriteBatch;
            //spriteBatch.End();
            //spriteBatch.Begin();
            // Draw background
            spriteBatch.Draw(_backgroundTexture, new Rectangle(100, 100, 400, 50), Color.Gray);
            // Draw input text
            spriteBatch.DrawString(_font, _inputText, new Vector2(110, 110), Color.Black);
          
        }

        public static void Cancel()
        {
            _tcs.SetResult(null);
        }
    }
}
