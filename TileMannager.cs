using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DinoGame
{
    public class TileMannager
    {
        private const int TileLines = 10;
        private const int TileRows = 10;

        private const int TilePerRowOnTimeSet = 8;
        private DinoGame Game { get; }
        public Vector2 Position;
        public Vector2 Speed = new Vector2(1, 0);

        private Vector2 PositionZero = Vector2.Zero;
        private Texture2D tilesTexture;
        private Random rand;
        private int[,] tileMatrix = new int[TileLines, TileRows];
        public Vector2 Center;

        public TileMannager(DinoGame game)
        {
            Game = game;
        }

        public void LoadContent()
        {
            var aseDoc = Game.Content.Load<AsepriteDocument>("tileSet");
            tilesTexture = aseDoc.Texture;

            rand = new Random();
            for (int i = 0; i < TileLines; i++)
            {
                for (int j = 0; j < TileRows; j++)
                {
                    if (j != 3)
                        tileMatrix[i, j] = -1;
                    else
                        tileMatrix[i, j] = rand.Next(-1, 8);
                }
            }

            Center = new Vector2(TileLines * Global.TileSize / 2, TileRows * Global.TileSize / 2);
        }

        public void Update(GameTime gameTime)
        {
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < TileLines; i++)
            {
                for (int j = 0; j < TileRows; j++)
                {
                    // get tile of the matrix
                    var tileId = tileMatrix[i, j];

                    // if has id on tileMatrix
                    if (tileId >= 0)
                    {
                        var tileX = tileId % TilePerRowOnTimeSet;
                        var tileY = tileId - (tileId % TilePerRowOnTimeSet);

                        // tile position on texture
                        var tileOriginRect = new Rectangle(tileX * Global.TileSize, tileY * Global.TileSize, Global.TileSize, Global.TileSize);
                        // draw the tile on screen
                        spriteBatch.Draw(tilesTexture, new Vector2(tileX * Global.TileSize, tileY * Global.TileSize), tileOriginRect, Color.White);
                    }
                }
            }

            if (Global.Debug)
            {
                for (int i = 0; i < TileLines; i++)
                {
                    for (int j = 0; j < TileRows; j++)
                    {
                        // tile position on screen
                        var rect = new Rectangle(Global.TileSize * i, Global.TileSize * j, Global.TileSize, Global.TileSize);
                        // draw the tile outline
                        PrimiviteDrawing.DrawRectangle(spriteBatch, rect, 1, Color.Green);
                    }
                }
            }
        }
    }
}
