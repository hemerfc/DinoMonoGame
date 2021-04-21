using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Documents;
using MonoGame.Aseprite.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DinoGame
{
    public class Player
    {
        private AnimatedSprite dinoSprite;

        public DinoGame Game { get; }

        private Vector2 PositionZero = Vector2.Zero;
        private Vector2 JumpSpeed = Vector2.Zero;
        private Vector2 JumpAcc = new Vector2(0, 2300f);
        private bool jumpLimit = false;
        private bool upButtomPrev = false;
        private bool jumping = false;
        private bool lowered = false;
        private float jumpSpeedLimit = -500f;

        public Player(DinoGame game)
        {
            Game = game;
        }

        public void LoadContent()
        {
            AsepriteDocument aseDoc = Game.Content.Load<AsepriteDocument>("dino");

            //  Create a new AnimatedSprite from the document
            dinoSprite = new AnimatedSprite(aseDoc);
        }

        public void SetStartPos(Vector2 startPos)
        {
            PositionZero = startPos;
            dinoSprite.Position = startPos;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool onFloor = dinoSprite.Position.Y >= PositionZero.Y;

            var upButtom = Keyboard.GetState().IsKeyDown(Keys.Up);
            var downButtom = Keyboard.GetState().IsKeyDown(Keys.Down);

            if ((upButtom && !upButtomPrev) && onFloor)
            {
                jumping = true;
            }
            if ((!upButtom && upButtomPrev))
            {
                jumping = false;
            }
            upButtomPrev = upButtom;

            lowered = downButtom && !jumping;

            // inicia um pulo se apertou agora, e se esta no chao
            if (jumping && !jumpLimit)
            {
                JumpSpeed -= JumpAcc * delta;
                jumpLimit = JumpSpeed.Y < jumpSpeedLimit;
                dinoSprite.Play("jumpUp");
            }
            else
            {
                if (!onFloor)
                {
                    JumpSpeed += JumpAcc * delta;
                    if (JumpSpeed.Y > 0)
                        dinoSprite.Play("jumpDown");
                }
                else
                {
                    JumpSpeed = Vector2.Zero;
                    //dinoSprite.Position = PositionZero; 
                    jumpLimit = false;
                    jumping = false;
                    if (!lowered)
                        dinoSprite.Play("run");
                    else
                        dinoSprite.Play("loweredRun");
                }
            }

            // dinoSprite.Position += JumpSpeed * delta;

            // TODO: Add your update logic here
            dinoSprite.Update(gameTime);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            dinoSprite.Render(spriteBatch);
        }

        // Move the player position based on input
        public void HandleInput(InputState inputState, PlayerIndex? controllingPlayer)
        {
            Vector2 playerMovement = Vector2.Zero;

            if (inputState.IsLeft(controllingPlayer))
            {
                playerMovement.X = -1;
            }
            else if (inputState.IsRight(controllingPlayer))
            {
                playerMovement.X = 1;
            }
            if (inputState.IsUp(controllingPlayer))
            {
                playerMovement.Y = -1;
            }
            else if (inputState.IsDown(controllingPlayer))
            {
                playerMovement.Y = 1;
            }
            else if (inputState.HasResetCommand(controllingPlayer))
            {
                dinoSprite.Position = PositionZero;
            }

            dinoSprite.Position += playerMovement;
            // move the camera
            //MoveCamera(cameraMovement, true);

        }
    }
}
