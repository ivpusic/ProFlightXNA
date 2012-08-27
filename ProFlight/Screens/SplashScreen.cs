using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Threading;
using System.Diagnostics;
using attackGame.Game_parts;


namespace attackGame
{
    public class SplashScreen : GameScreen
    {
        public static bool drawAnimation = true;
        Texture2D title;
        Texture2D background;
        DispatcherTimer timer;
        Texture2D splashTexture;
        SplashAnimation splashAnimation;
        ScreenSplash player;
        Vector2 splashPosition;
        public SplashScreen()
        {
            //TransitionOnTime = TimeSpan.FromSeconds(0);
            //TransitionOffTime = TimeSpan.FromSeconds(0);
            splashPosition = new Vector2(240, 400);
        }

        public override void LoadContent()
        {
            player = new ScreenSplash();
            //title = ScreenManager.Game.Content.Load<Texture2D>("title");
            background = ScreenManager.Game.Content.Load<Texture2D>("bgr");
            splashTexture = ScreenManager.Game.Content.Load<Texture2D>("backgroundScreen");
            splashAnimation = new SplashAnimation();
            splashAnimation.Initialize(splashTexture, Vector2.Zero, 284, 115, 7, 30, Color.White, 1f, true);
            player.Initialize(splashAnimation, splashPosition);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //player.Position = splashPosition;
            player.Update(gameTime);
            //base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            
            // Draw Background
         
            spriteBatch.Draw(background, new Vector2(0, 0),
                 Color.Black);
            if(drawAnimation) player.Draw(spriteBatch);
            // Draw Title
            //spriteBatch.Draw(title, new Vector2(60, 55),
              //   new Color(255, 255, 255, TransitionAlpha));

            spriteBatch.End();
        }

        

    }
}
