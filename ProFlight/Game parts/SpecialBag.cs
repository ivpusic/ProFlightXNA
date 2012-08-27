using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace attackGame.Game_parts
{
    class SpecialBag
    {
        // Image representing the health
        public Texture2D Texture;

        // Position of the health relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the health can inflict to an enemy
        public int Damage;

        // Represents the viewable boundary of the game
        Viewport viewport;

        // Get the width of the health ship
        public int Width
        {
            get { return Texture.Width; }
        }

        // Get the height of the health ship
        public int Height
        {
            get { return Texture.Height; }
        }

        public int health = 15;

        // Determines how fast the health moves
        float bulletsMoveSpeed;

        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            Damage = 0;

            bulletsMoveSpeed = 10f;
        }

        public void Update()
        {
            // Projectiles always move to the right
            Position.Y -= bulletsMoveSpeed;

            // Deactivate the bullet if it goes out of screen
            if (Position.Y < 5 || health <= 0)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 1.55f,
            new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
