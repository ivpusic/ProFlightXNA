using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using System.Windows.Threading;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using Microsoft.Xna.Framework.Input.Touch;
using attackGame.Screens;
using Microsoft.Phone.Net.NetworkInformation;

namespace attackGame
{
    class HighScoreScreen : GameScreen
    {
        Texture2D background;
        SpriteFont font;
        int score;
        public List<HighScore> temp;
        ISHelper isoHelper;
        int width, height;
        string adresa;
        string saveAdress;
        WebClient getHighScores;
        private AsyncCallback OnEndDialog;
        bool first = true;
        string arkaScore;
        string arkaPlayer;
        bool repeat = true;
        public HighScoreScreen()
        {

            adresa = "http://arka.foi.hr/~ivpusic/proflight/readScores.php";
            saveAdress = "http://arka.foi.hr/~ivpusic/proflight/saveScores.php";

            temp = new List<HighScore>();
            isoHelper = new ISHelper();
            this.width = 480;
            this.height = 800;
        }

        public void SaveHighSCoresOnArka()
        {
            if (repeat)
            {
                repeat = false;
                string argumenti2 = "scores=" + arkaScore + "&" + "player=" + arkaPlayer;
                saveAdress += "?" + argumenti2;
                WebClient sp = new WebClient();
                Uri uriAdresa = new Uri(saveAdress, UriKind.Absolute);
                const string data = "nesto";
                sp.UploadStringAsync(uriAdresa, data);
                sp.UploadStringCompleted += new UploadStringCompletedEventHandler(sp_UploadStringCompleted);
            }
        }

        void sp_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Guide.BeginShowMessageBox("No errors", "Sync successful", new string[] { "OK" }, 0, MessageBoxIcon.None, OnEndDialog, null);
            repeat = true;
        }

        public void GetScoresFromArka()
        {
            getHighScores = new WebClient();
            Uri uriAdresa = new Uri(adresa, UriKind.Absolute);
            getHighScores.DownloadStringAsync(uriAdresa);
            getHighScores.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getHighScores_DownloadStringCompleted);
        }

        void getHighScores_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string result = e.Result.ToString();
            Debug.WriteLine(result);
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu");
            font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
            temp = isoHelper.LoadHighScores("scoress.xml");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

       

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), null, new Color(255, 255, 255, TransitionAlpha), 0f, Vector2.Zero, 1.01f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Your best scores:", new Vector2(width / 8, height / 6), Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);
            int tempHeight = height;
            int i = 1;
            foreach (HighScore hs in temp)
            {
                if (first)
                {
                    arkaScore = hs.Score.ToString();
                    arkaPlayer = hs.Player;
                    first = false;
                }
                spriteBatch.DrawString(font, i++ + ". " + hs.Player + ": " + hs.Score , new Vector2(width / 7, tempHeight / 4), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                tempHeight += 180;
            }
            spriteBatch.DrawString(font, "Click here to view\nworld high scores:", new Vector2(10,700), Color.Red, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Click here to sync your\n         scores with\n    world high scores:", new Vector2(250, 700), Color.Red, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            
            TouchCollection touchState;
            touchState = TouchPanel.GetState();
            
            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:
                        if (location.Position.X >= 10 &&
                    location.Position.Y >= 600 &&
                    location.Position.X <= 200 &&
                    location.Position.Y <= 800)
                    {
                        if (!NetworkInterface.GetIsNetworkAvailable())
                        {
                            Guide.BeginShowMessageBox("Error", "Network unavailable", new string[] { "OK" }, 0, MessageBoxIcon.None, OnEndDialog, null);
                            break;
                        }
                        else
                        {
                            ExitScreen();
                            ScreenManager.AddScreen(new WorldScoresScreen());
                            break;
                        }
                    }

                    if (location.Position.X >= 250 &&
                    location.Position.Y >= 600 &&
                    location.Position.X <= 480 &&
                    location.Position.Y <= 800)
                    {
                        Debug.WriteLine("zapisuje");
                        if (!NetworkInterface.GetIsNetworkAvailable())
                        {
                            Guide.BeginShowMessageBox("Error", "Network unavailable", new string[] { "OK" }, 0, MessageBoxIcon.None, OnEndDialog, null);
                            break;
                        }
                        else
                        {
                            if (repeat)
                            {
                                //repeat = false;
                                SaveHighSCoresOnArka();
                            }
                            break;
                        }
                    }
                        break;
                }  
            }
            if (input.PauseGame)
            {
                ExitScreen();
                //ScreenManager.AddScreen(new PhoneMainMenu());
            }
        
        }



    }

}
