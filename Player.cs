﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Win8ShooterGame;

namespace Shooter
{
    class Player
    {
        //Animation representing the player
        //public Texture2D PlayerTexture;
        public Animation PlayerAnimation;

        //Poision of the Player relative to the upper left side if the screen
        public Vector2 Position;

        //State of the player
        public bool Active;

        //Amount of hit points that player has
        public int Health;

        //Get the width of the player ship
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        //Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public void Initialize(Animation animation, Vector2 position)
        {

            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}
