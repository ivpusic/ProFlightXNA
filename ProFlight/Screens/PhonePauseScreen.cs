#region File Description
//-----------------------------------------------------------------------------
// PhonePauseScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace attackGame
{
    /// <summary>
    /// A basic pause screen for Windows Phone
    /// </summary>
    class PhonePauseScreen : PhoneMenuScreen
    {
        Texture2D background;
        public PhonePauseScreen()
            : base("")
        {
            // Create the "Resume" and "Exit" buttons for the screen

            Button resumeButton = new Button("Resume", "reset");
            resumeButton.Tapped += resumeButton_Tapped;
            MenuButtons.Add(resumeButton);

            Button exitButton = new Button("Main menu", "reset");
            exitButton.Tapped += exitButton_Tapped;
            MenuButtons.Add(exitButton);
        }

        /// <summary>
        /// The "Resume" button handler just calls the OnCancel method so that 
        /// pressing the "Resume" button is the same as pressing the hardware back button.
        /// </summary>
        void resumeButton_Tapped(object sender, EventArgs e)
        {
            ExitScreen();
            GameplayHelper.isPauseGame = false;
            GameplayHelper.updateGameTime = true;
            //GameplayHelper.PlayMusic(GameplayHelper.gameplayMusic);
            GameplayHelper.ResumeSong();
        }

        public override void HandleInput(AlienGameSample.InputState input)
        {
            if (input.PauseGame)
            {
                ExitScreen();
                GameplayHelper.isPauseGame = false;
                GameplayHelper.updateGameTime = true;
                //GameplayHelper.PlayMusic(GameplayHelper.gameplayMusic);
                GameplayHelper.ResumeSong();
            }
            base.HandleInput(input);
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu");
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), null, new Color(255, 255, 255, TransitionAlpha), 0f, Vector2.Zero, 1.01f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// The "Exit" button handler uses the LoadingScreen to take the user out to the main menu.
        /// </summary>
        void exitButton_Tapped(object sender, EventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new PhoneMainMenu());
            //LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
            //                                               new PhoneMainMenuScreen());
        }



        protected override void OnCancel()
        {
            ExitScreen();
            base.OnCancel();
        }
    }
}
