using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

public static class SpriteBatchExtensions
{
    public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float thickness, float scale)
    {
        // 计算圆周上的点，并应用缩放
        Vector2[] vertex = new Vector2[sides];
        float step = MathHelper.TwoPi / sides; // 分割圆周
        float scaledRadius = radius * scale; // 应用缩放

        for (int i = 0; i < sides; i++)
        {
            float rad = step * i;
            vertex[i] = center + new Vector2((float)Math.Cos(rad) * scaledRadius, (float)Math.Sin(rad) * scaledRadius);
        }

        // 连接点以绘制圆
        for (int i = 0; i < sides; i++)
        {
            int j = (i + 1) % sides;
            DrawLine(spriteBatch, vertex[i], vertex[j], color, thickness * scale); // 应用缩放到线的厚度
        }
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
    {
        Vector2 edge = end - start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);
        float length = edge.Length();

        spriteBatch.Draw(
            TextureManager.GetWhitePixel(spriteBatch.GraphicsDevice),
            new Rectangle((int)start.X, (int)start.Y, (int)length, (int)thickness),
            null,
            color,
            angle,
            new Vector2(0, 0.5f),
            SpriteEffects.None,
            0);
    }
}

public static class TextureManager
{
    private static Texture2D whitePixel;

    public static Texture2D GetWhitePixel(GraphicsDevice graphicsDevice)
    {
        if (whitePixel == null)
        {
            whitePixel = new Texture2D(graphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });
        }
        return whitePixel;
    }
}
