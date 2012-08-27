using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace attackGame.Game_parts
{
    class Parallaxingbackground
    {
        // The image representing the parallaxing background
        Texture2D texture;

        // An array of positions of the parallaxing background
        Vector2[] positions;

        // The speed which the background is moving
        int speed;
        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            // Load the background texture we will be using
            texture = content.Load<Texture2D>(texturePath);

            // Set the speed of the background
            this.speed = speed;

            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            //positions = new Vector2[screenWidth / texture.Width + 1];
            positions = new Vector2[2];
            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                //positions[i] = new Vector2(i * texture.Width, 0);
                positions[i] = new Vector2(480, texture.Width * i);
            }
        }

        public void Update()
        {
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the speed
                positions[i].Y -= speed;
                // If the speed has the background moving to the left
                if (speed <= 0) 
                {
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (positions[i].Y <= texture.Width)
                    {
                        positions[i].Y = texture.Width * (positions.Length - 1);
                    }
                }

                // If the speed has the background moving to the right
                else
                {
                    if (positions[i].Y == -texture.Width)
                    {
                        positions[i].Y = texture.Width * (positions.Length - 1);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                //spriteBatch.Draw(texture, positions[i], Color.White);
                spriteBatch.Draw(texture, positions[i], null, Color.White, 1.57f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

    }
}
