using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;
using System;

namespace DinoGame
{
    public class DinoGame : Game
    {
        private GraphicsDeviceManager graphics;
        private InputState inputState;
        private SpriteBatch spriteBatch;
        private Player player;
        private TileMannager groundTiles;
        private RenderTarget2D renderTarget;
        int renderWidthDesired = 640;
        int renderHeightDesired = 480;
        bool ResizePenging = false;

        public DinoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            ResizePenging = false;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (s, e) => { ResizePenging = true; };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            inputState = new InputState();
        }

        protected override void Initialize()
        {
            player = new Player(this);
            groundTiles = new TileMannager(this);

            base.Initialize();

            PrimiviteDrawing.Init(GraphicsDevice);

            PrepareRenderTarget();
        }

        private void PrepareRenderTarget()
        {
            var renderHeightComplement = (Window.ClientBounds.Height % renderHeightDesired) / (Window.ClientBounds.Height / renderHeightDesired);
            var renderWidthComplement = (Window.ClientBounds.Width % renderWidthDesired) / (Window.ClientBounds.Width / renderWidthDesired);
            var renderHeight = renderHeightDesired + renderHeightComplement;
            var renderWidth = renderWidthDesired + renderWidthComplement;

            //Global.Camera.ViewportWidth = renderWidth;
            //Global.Camera.ViewportHeight = renderHeight;

            //renderTarget = new RenderTarget2D(GraphicsDevice, renderWidth, renderHeight, false, SurfaceFormat.Bgr565, DepthFormat.Depth24Stencil8);

            Global.Camera.ViewportWidth = renderWidthDesired;
            Global.Camera.ViewportHeight = renderHeightDesired;
            renderTarget = new RenderTarget2D(GraphicsDevice, renderWidthDesired, renderHeightDesired, false, SurfaceFormat.Bgr565, DepthFormat.Depth24Stencil8);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            groundTiles.LoadContent();
            player.LoadContent();

            //Global.Camera.CenterOn(Vector2.Zero);
        }

        protected override void Update(GameTime gameTime)
        {
            if (ResizePenging)
            {
                ResizePenging = false;
                PrepareRenderTarget();
            }

            inputState.Update();
            Global.Camera.HandleInput(inputState, PlayerIndex.One);
            player.HandleInput(inputState, PlayerIndex.One);
            HandleInput(inputState, PlayerIndex.One);

            if (inputState.IsExitGame(PlayerIndex.One))
            {
                Exit();
            }

            groundTiles.Update(gameTime);
            player.Update(gameTime);

            base.Update(gameTime);
        }

        private void HandleInput(InputState inputState, PlayerIndex one)
        {
            if (inputState.HasDebugCommand(PlayerIndex.One))
            {
                Global.Debug = !Global.Debug;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            // PRE RENDER
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Global.Camera.TranslationMatrix);

            groundTiles.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);

            Global.Camera.Draw(spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null); // Back buffer


            // SCREEN RENDER
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            var renderHeightComplement = (Window.ClientBounds.Height % renderHeightDesired) / (Window.ClientBounds.Height / renderHeightDesired);
            var renderWidthComplement = (Window.ClientBounds.Width % renderWidthDesired) / (Window.ClientBounds.Width / renderWidthDesired);
            spriteBatch.Draw(renderTarget, new Vector2(renderWidthComplement / 2, renderHeightComplement / 2), Color.White);

            //spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);

            if (Global.Debug)
            {
                // draw rect in safe draw area
                var rect = new Rectangle(renderWidthComplement / 2, renderHeightComplement / 2, renderWidthDesired, renderHeightDesired);
                PrimiviteDrawing.DrawRectangle(spriteBatch, rect, 1, Color.Red);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
