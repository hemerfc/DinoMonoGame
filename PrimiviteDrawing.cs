using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DinoGame
{
    static public class PrimiviteDrawing
    {
        static public Texture2D WhitePixel { get; set; }

        static public void Init(GraphicsDevice graphicsDevice)
        {
            WhitePixel = new Texture2D(graphicsDevice, 1, 1);
            WhitePixel.SetData(new[] { Color.White });
        }

        static public void DrawRectangle(SpriteBatch batch, Rectangle area, int lineWidth, Color color)
        {
            if (WhitePixel == null)
                throw new Exception("This method requires that the WhitePixel property is set.");

            batch.Draw(WhitePixel, new Rectangle(area.X, area.Y, area.Width, lineWidth), color);
            batch.Draw(WhitePixel, new Rectangle(area.X, area.Y, lineWidth, area.Height), color);
            batch.Draw(WhitePixel, new Rectangle(area.X + area.Width - lineWidth, area.Y, lineWidth, area.Height), color);
            batch.Draw(WhitePixel, new Rectangle(area.X, area.Y + area.Height - lineWidth, area.Width, lineWidth), color);
        }

        static public void DrawRectangle(SpriteBatch batch, Rectangle area)
        {
            DrawRectangle(batch, area, 1, Color.White);
        }

        public static void DrawCircle(SpriteBatch spritbatch, Vector2 center, float radius, Color color, int lineWidth = 2, int segments = 16)
        {
            Vector2[] vertex = new Vector2[segments];

            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for (int i = 0; i < segments; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(spritbatch, vertex, segments, color, lineWidth);
        }

        public static void DrawPolygon(SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if (count > 0)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    DrawLineSegment(spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }
        public static void DrawLineSegment(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {
            if (WhitePixel == null)
                throw new Exception("This method requires that the WhitePixel property is set.");

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(WhitePixel, point1, null, color,
            angle, Vector2.Zero, new Vector2(length, lineWidth),
            SpriteEffects.None, 0f);
        }
    }
}
