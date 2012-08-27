using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace attackGame.Game_parts
{
    class Stone
    {
        // Animation representing the player
        public Animation StoneAnimation;

        // Animation representing the player
        //public Texture2D PlayerTexture;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;

        // Amount of hit points that player has
        public int Health;

        // Get the width of the player ship
        public int Width
        {
            get { return StoneAnimation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return StoneAnimation.FrameHeight; }
        }
        float stoneMoveSpeed = 20f;
        public int Damage = 1000;
        // Initialize the player
        public void Initialize(Animation animation, Vector2 position)
        {
            StoneAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 1000;
        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.Y -= stoneMoveSpeed;

            // Update the position of the Animation
            StoneAnimation.Position = Position;

            // Update Animation
            StoneAnimation.Update(gameTime);

            // If the enemy is past the screen or its health reaches 0 then deactivateit
            if (Position.Y < - 200 || Health <= 0)
            {
                // By setting the Active flag to false, the game will remove this objet from the 
                // active game list
                Active = false;
            }
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            StoneAnimation.Draw(spriteBatch);
        }
    }
}
