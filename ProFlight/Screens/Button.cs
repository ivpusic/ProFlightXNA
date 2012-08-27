#region File Description
//-----------------------------------------------------------------------------
// Button.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AlienGameSample;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace attackGame
{
    /// <summary>
    /// A special button that handles toggling between "On" and "Off"
    /// </summary>
    class BooleanButton : Button
    {
        private string option;
        public bool value;

        /// <summary>
        /// Creates a new BooleanButton.
        /// </summary>
        /// <param name="option">The string text to display for the option.</param>
        /// <param name="value">The initial value of the button.</param>
        public BooleanButton(string option, bool value, string path)
            : base(option)
        {
            this.option = option;
            this.value = value;
            this.Path = path;
            GenerateText();
        }

        protected override void OnTapped()
        {
            // When tapped we need to toggle the value and regenerate the text
            value = !value;
            GenerateText();

            base.OnTapped();
        }

        /// <summary>
        /// Helper that generates the actual Text value the base class uses for drawing.
        /// </summary>
        private void GenerateText()
        {
            Text = string.Format("{0}: {1}", option, value ? "On" : "Off");
           
        }
    }

    /// <summary>
    /// Represents a touchable button.
    /// </summary>
    class Button 
    {

        ContentManager content;

        /// <summary>
        /// The text displayed in the button.
        /// </summary>
        public string Text = "Button";

        /// <summary>
        /// The position of the top-left corner of the button.
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// The size of the button.
        /// </summary>
        public Vector2 Size = new Vector2(250, 75);

        /// <summary>
        /// The thickness of the border drawn for the button.
        /// </summary>
        public int BorderThickness = 4;

        /// <summary>
        /// The color of the button border.
        /// </summary>
        public Color BorderColor = new Color(200, 200, 200);

        /// <summary>
        /// The color of the button background.
        /// </summary>
        public Color FillColor = new Color(100, 100, 100) * .75f;

        //public Color FillColor = Color.Black;

        /// <summary>
        /// The color of the text.
        /// </summary>
        public Color TextColor = Color.Black;

        /// <summary>
        /// The opacity of the button.
        /// </summary>
        public float Alpha = 0f;

        /// <summary>
        /// Poziva se kada je korisnik kliknuo na button
        /// </summary>
        public event EventHandler<EventArgs> Tapped;
        public string Path;

        /// <summary>
        /// Konstruktor klase button. Postavlja tekst i putanju do slike koja æe predstavljati button
        /// </summary>
        /// <param name="text">Text buttona</param>
        /// <param name="path">Putanja do slike</param>
        public Button(string text, string path)
        {
            this.Text = text;
            this.Path = path;
        }

        public Button(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Pozivanje eventa "Tapped"
        /// </summary>
        protected virtual void OnTapped()
        {
            if (Tapped != null)
                Tapped(this, EventArgs.Empty);
        }

        /// <summary>
        /// Proslijeðuje poziciju gdje je korisnik kliknuo na ekran
        /// </summary>
        /// <param name="tap">Pozicija</param>
        /// <returns>True ukoliko je kliknuo na instancu buttona, false ukoliko nije</returns>
        public bool HandleTap(Vector2 tap)
        {
            if (tap.X >= Position.X &&
                tap.Y >= Position.Y &&
                tap.X <= Position.X + Size.X &&
                tap.Y <= Position.Y + Size.Y)
            {
                OnTapped();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="screen">The screen drawing the button</param>
        public void Draw(GameScreen screen)
        {
            // Grab some common items from the ScreenManager
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
            //SpriteFont font = screen.ScreenManager.Font;
            SpriteFont font = LoadingScreen.cont.Load<SpriteFont>("dobarFontJe");
            Texture2D  blank= screen.ScreenManager.BlankTexture;

            // Compute the button's rectangle
            Rectangle r = new Rectangle(

                (int)Position.X + 15,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y);
            // Fill the button
            //spriteBatch.Draw(blank, r, FillColor * Alpha);
            
            Texture2D texture = LoadingScreen.cont.Load<Texture2D>(Path);
            spriteBatch.Draw(texture,r, Color.White);

            // Draw the text centered in the button
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPosition = new Vector2(r.Center.X, r.Center.Y);
            textPosition.X = (int)textPosition.X - 110;
            textPosition.Y = (int)textPosition.Y;
            //spriteBatch.DrawString(font, Text, textPosition, TextColor * Alpha);
            spriteBatch.DrawString(font, Text, textPosition, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }
    }
}
