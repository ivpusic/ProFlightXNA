using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using AlienGameSample;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Phone.Net.NetworkInformation;

namespace attackGame.Screens
{
    class DemoScreen : GameScreen
    {
        public SpriteFont font;
        Texture2D bck;
        private AsyncCallback OnEndDialog;
        public DemoScreen()
        {

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(bck, new Vector2(480, 0), null, new Color(255, 255, 255, TransitionAlpha), 1.57f, Vector2.Zero, 1.01f, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            TouchCollection touchState;
            touchState = TouchPanel.GetState();
            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:

                        if (location.Position.X >= 0 &&
                location.Position.Y >= 200 &&
                location.Position.X <= 100 &&
                location.Position.Y <= 700)
                        {
                            this.ExitScreen();
                            ScreenManager.AddScreen(new PhoneMainMenu());
                        }

                        if (location.Position.X >= 150 &&
                location.Position.Y >= 200 &&
                location.Position.X <= 300 &&
                location.Position.Y <= 700)
                        {
                            if (!NetworkInterface.GetIsNetworkAvailable())
                            {
                                Guide.BeginShowMessageBox("Error", "Network unavailable", new string[] { "OK" }, 0, MessageBoxIcon.None, OnEndDialog, null);
                                break;
                            }
                            else
                            {
                                MarketplaceDetailTask task = new MarketplaceDetailTask();
                                task.ContentType = MarketplaceContentType.Applications;
                                task.ContentIdentifier = "b9141a04-bc46-408a-a761-1307a43a9b2c";
                                task.Show();
                                this.ExitScreen();
                                ScreenManager.AddScreen(new PhoneMainMenu());
                            }
                        }
                        break;
                }
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input.PauseGame)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new PhoneMainMenu());
            }
            base.HandleInput(input);
        }


        public override void LoadContent()
        {
            bck = ScreenManager.Game.Content.Load<Texture2D>("endMenu_demo");
            font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
            base.LoadContent();
        }

    }
}
