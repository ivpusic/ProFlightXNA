using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AlienGameSample;
using System.Net;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace attackGame.Screens 
{
    class WorldScoresScreen : GameScreen
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
        public WorldScoresScreen()
        {
            temp = new List<HighScore>();
            isoHelper = new ISHelper();
            this.width = 480;
            this.height = 800;
            Debug.WriteLine("u konstruktoru je");
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu");
            font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
            //temp = isoHelper.LoadHighScores("scoress.xml");
            adresa = "http://arka.foi.hr/~ivpusic/proflight/readScores.php";
            try
            {
                GetScoresFromArka();
            }
            catch (Exception ex)
            {
                
            }
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
            XElement scores = XElement.Parse(@result);

            var studenti = from sc in scores.Descendants("scores")
                           select new HighScore
                           {
                               Score = Convert.ToInt32(sc.Element("score").Value),
                               Player = sc.Element("player").Value
                           };
            foreach(HighScore s in studenti)
            {
                temp.Add(s);
            }
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
            spriteBatch.DrawString(font, "World best scores:", new Vector2(width / 8, height / 6), Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);
            int tempHeight = height;
            int i = 1;
            foreach (HighScore hs in temp)
            {
                spriteBatch.DrawString(font, i++ + ". " + hs.Player + ": " + hs.Score, new Vector2(width / 7, tempHeight / 4), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
                tempHeight += 180;
            }
            spriteBatch.End();
        }

        public override void HandleInput(InputState input)
        {
            if (input.PauseGame)
            {
                ExitScreen();
            }
        }


    }
}
