using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace attackGame
{
    class PauseScreen : PhoneMenuScreen
    {
        Texture2D background;
        public PauseScreen()
            : base("Pause")
        {
           
            // Create our menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Return");
            MenuEntry highScoreEntry = new MenuEntry("Exit");

            Button resume = new Button("", "button_resume");
            resume.Tapped += resume_Tapped;
            Button mainMenu = new Button("", "button_resume");
            mainMenu.Tapped += mainMenu_Tapped;
            //startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            //highScoreEntry.Selected += OnCancel;

            // Add entries to the menu.
            //MenuEntries.Add(startGameMenuEntry);
            //MenuEntries.Add(highScoreEntry);
        }

        void resume_Tapped(object sender, EventArgs e)
        {
            this.ExitScreen();
        }

        void mainMenu_Tapped(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new PhoneMainMenu());
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), null, new Color(255, 255, 255, TransitionAlpha), 0f, Vector2.Zero, 1.01f, SpriteEffects.None, 0);
            spriteBatch.End();
            //base.Draw(gameTime);
        }

        //void StartGameMenuEntrySelected(object sender, EventArgs e)
        //{

        //    ScreenManager.AddScreen(new GameplayScreen());
        //}

        //protected override void OnCancel()
        //{
        //    ScreenManager.AddScreen(new LoadingScreen(1,2));
        //    //ScreenManager.Game.Exit();
        //}
    
    }
}
