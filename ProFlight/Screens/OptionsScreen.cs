using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AlienGameSample;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace attackGame
{
    class OptionsScreen : PhoneMenuScreen
    {
        List<bool> options;
        BooleanButton music, vibration, menuMusic;
        ISOptions iso;
        Texture2D background;
        bool _music = true;
        bool _vibration = true;
        bool _menuMusic = true;

        public OptionsScreen()
            : base("")
        {
            iso = new ISOptions();
            options = new List<bool>();
            options = iso.LoadOptions("options.xml");
            if(options.Count > 0)
            {
                //indeks 0 - opcija ukljuci/isljuci muziku
                //indeks 1 - opcija ukljuci/iskljuci glazbu
                //indeks 2 - opcija ukljuci/iskljuci menu muziku
                _music = options[0];
                _menuMusic = options[2];
                _vibration = options[1];
            }
            music = new BooleanButton("Game Music", _music, "reset");
            music.Tapped += music_Tapped;
            MenuButtons.Add(music);

            menuMusic = new BooleanButton("Menu Music", _menuMusic, "reset");
            menuMusic.Tapped += MenuMusic_Tapped;
            MenuButtons.Add(menuMusic);

            vibration = new BooleanButton("Vibration", _vibration, "reset");
            vibration.Tapped += vibration_Tapped;
            MenuButtons.Add(vibration);
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu");
            base.LoadContent();
        }
        float alpha = 1;
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();


            spriteBatch.Draw(background, new Vector2(0, 0),
                 new Color(255, 255, 255, alpha));
            alpha -= 0.01f;

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void music_Tapped(object sender, EventArgs e)
        {
            BooleanButton button = sender as BooleanButton;
            // In a real game, you'd want to store away the value of 
            // the button to turn off sounds here. :)
            
        }

        void MenuMusic_Tapped(object sender, EventArgs e)
        {
            BooleanButton button = sender as BooleanButton;
            // In a real game, you'd want to store away the value of 
            // the button to turn off sounds here. :)
            PhoneMainMenu.checkSetting = true;

        }

        void vibration_Tapped(object sender, EventArgs e)
        {
            BooleanButton button = sender as BooleanButton;
            // In a real game, you'd want to store away the value of 
            // the button to turn off sounds here. :)
        }

        public override void HandleInput(InputState input)
        {
            TouchCollection touchState;
         
            touchState = TouchPanel.GetState();

            if (input.PauseGame)
            {
                options.Clear();
                options.Add(Convert.ToBoolean(music.value));
                options.Add(Convert.ToBoolean(vibration.value));
                options.Add(Convert.ToBoolean(menuMusic.value));
                iso.SaveOptions("options.xml", options);
                ExitScreen();
            }

            //interpert touch screen presses
            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:

                        foreach (Button b in MenuButtons)
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
        }

    }
}
