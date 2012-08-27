using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;

namespace attackGame.Screens
{
    class HelpScreen : GameScreen
    {
        public SpriteFont font;
        Texture2D bck;
        bool newGame = false;
        Texture2D help1, help2, help3;
        int i = 0;
        public HelpScreen()
        {

        }

        public HelpScreen(bool NewGame)
        {
            this.newGame = NewGame;
        }

        public override void LoadContent()
        {
            bck = ScreenManager.Game.Content.Load<Texture2D>("bgr");
            help1 = ScreenManager.Game.Content.Load<Texture2D>("helpWindow1");
            help2 = ScreenManager.Game.Content.Load<Texture2D>("helpWindow2");
            help3 = ScreenManager.Game.Content.Load<Texture2D>("helpWindow3");
            //font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            TouchCollection touchState;
            touchState = TouchPanel.GetState();
            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:

                        if (location.Position.X >= 10 &&
                location.Position.Y >= 650 &&
                location.Position.X <= 100 &&
                location.Position.Y <= 800 && i == 2)
                        {
                            this.ExitScreen();
                            if (newGame) ScreenManager.AddScreen(new ReadyScreen());
                      
                        }

                        if (location.Position.X >= 10 &&
                    location.Position.Y >= 50 &&
                    location.Position.X <= 100 &&
                    location.Position.Y <= 150 && !(i == 2))
                        {
                            this.ExitScreen();
                            if (newGame) ScreenManager.AddScreen(new ReadyScreen());
                         
                        }

                    

                        if (location.Position.X >= 10 &&
                    location.Position.Y >= 650 &&
                    location.Position.X <= 100 &&
                    location.Position.Y <= 800 && !(i == 2))
                        {
                            i++;
                        }
                        break;
                }
            }
            if (touchState.Count > 0)
            {
                
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input.PauseGame)
            {
                ExitScreen();
                if (newGame) ScreenManager.AddScreen(new ReadyScreen());
                //else ExitScreen();
            }
            base.HandleInput(input);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(bck, new Vector2(0, 0), null, new Color(255, 255, 255, TransitionAlpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            if(i == 0)
            spriteBatch.Draw(help1, new Vector2(480, 0), null, new Color(255, 255, 255, TransitionAlpha), 1.57f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            if (i == 1)
                spriteBatch.Draw(help2, new Vector2(480, 0), null, new Color(255, 255, 255, TransitionAlpha), 1.57f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            if (i == 2)
                spriteBatch.Draw(help3, new Vector2(480, 0), null, new Color(255, 255, 255, TransitionAlpha), 1.57f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(font, "", new Vector2(270, 330), Color.White, 1.57f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
