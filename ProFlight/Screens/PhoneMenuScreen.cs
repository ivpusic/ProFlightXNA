#region File Description
//-----------------------------------------------------------------------------
// PhoneMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using AlienGameSample;
using System.Diagnostics;

namespace attackGame
{
    /// <summary>
    /// Provides a basic base screen for menus on Windows Phone leveraging the Button class.
    /// </summary>
    class PhoneMenuScreen : GameScreen
    {
        public List<Button> menuButtons = new List<Button>();
        string menuTitle;
        Player player;
        InputAction menuCancel;
        Texture2D playerTexture;
        /// <summary>
        /// Gets the list of buttons, so derived classes can add or change the menu contents.
        /// </summary>
        public IList<Button> MenuButtons
        {
            get { return menuButtons; }
        }

        /// <summary>
        /// Creates the PhoneMenuScreen with a particular title.
        /// </summary>
        /// <param name="title">The title of the screen</param>
        public PhoneMenuScreen(string title)
        {
            menuTitle = title;

            //TransitionOnTime = TimeSpan.FromSeconds(0);
            //TransitionOffTime = TimeSpan.FromSeconds(0);

            // Create the menuCancel action
            menuCancel = new InputAction(new Buttons[] { Buttons.Back }, null, true);

            // We need tap gestures to hit the buttons
            EnabledGestures = GestureType.Tap;
        }

        public override void Activate(bool instancePreserved)
        {
            // When the screen is activated, we have a valid ScreenManager so we can arrange
            // our buttons on the screen                                                 
            float y = 200f;
            float center = ScreenManager.GraphicsDevice.Viewport.Bounds.Center.X;
            float x = ScreenManager.GraphicsDevice.Viewport.X / 2;
            for (int i = 0; i < MenuButtons.Count; i++)
            {
                Button b = MenuButtons[i];
                
                b.Position = new Vector2(100, y);
                //Debug.WriteLine(b.Position.X + " " + b.Position.Y);
                y += b.Size.Y * 1.2f;
            }

            base.Activate(instancePreserved);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
             //Update opacity of the buttons
            foreach (Button b in menuButtons)
            {
                b.Alpha = TransitionAlpha;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// An overrideable method called whenever the menuCancel action is triggered
        /// </summary>
        protected virtual void OnCancel() { }

        public override void HandleInput(InputState input)
        {
            TouchCollection touchState;
            touchState = TouchPanel.GetState();

            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:

                        foreach (Button b in menuButtons)
                        {
                            b.HandleTap(location.Position);
                        }
                        break;
                    case TouchLocationState.Moved:
                        break;
                    case TouchLocationState.Released:
                        break;
                }
            }

            base.HandleInput(input);
        }

        public override void LoadContent()
        {
            //player = new Player();
            //playerTexture = LoadingScreen.cont.Load<Texture2D>("shipAnimation");
            //Animation playerAnimation = new Animation();
            //playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
            //player.Initialize(playerAnimation, new Vector2(30,30));
            base.LoadContent();
        }
        

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();
            // Draw all of the buttons
            foreach (Button b in menuButtons)
                b.Draw(this);
            
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
