using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace attackGame
{
    class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        // The scale used to display the sprite strip
        float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime,
            Color color, float scale, bool looping)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // Postavljanje vremena koja se prate na 0
            elapsedTime = 0;
            currentFrame = 0;

            // Postavljanje stanja animacije na "Aktivno" -> Po defaultu
            Active = true;
        }


        public void Update(GameTime gameTime)
        {
            // Ukoliko animacija nije aktivna, nemoj update-at
            if (Active == false)
                return;

            // Update-aj proteklo vrijeme
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Ukoliko je proteklo vrijeme veæe od vremena jednog frame-a
            // trebamo zamjeniti frame
            if (elapsedTime > frameTime)
            {
                // Pomicanje na iduci frame
                currentFrame++;

                // Ukoliko smo dosli do kraja frame-ova vrati nas na 1, odnosno na 0-ti frame
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // Ukoliko vise nismo u igri, ugasi animaciju
                    if (Looping == false)
                        Active = false;
                }

                // Resetiraj proteklo vrijeme
                elapsedTime = 0;
            }

            // Uzimanje ispravnog fram-a iz niza framew-ova
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Postavi frame na pravo mjesto
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
            (int)Position.Y - (int)(FrameHeight * scale) / 2,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Ukoliko je animacija aktivna
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, 1.55f, Vector2.Zero, SpriteEffects.None, 0);
            }
        }

    }
}
