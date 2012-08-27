using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;


namespace attackGame
{
    class GameOverScreen : GameScreen
    {
        Texture2D background;
        SpriteFont font;
        DispatcherTimer timer;
        int score;
        public GameOverScreen(int score)
        {
            //TransitionOnTime = TimeSpan.FromSeconds(0);
            //TransitionOffTime = TimeSpan.FromSeconds(0);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            this.score = score;
        }

        public GameOverScreen()
        {

        }

        void timer_Tick(object sender, EventArgs e)
        {
            ExitScreen();
            timer.Stop();
            PhoneMainMenu.checkSetting = true;
            ScreenManager.AddScreen(new PhoneMainMenu());
        }

        public override void LoadContent()
        {
            background = ScreenManager.Game.Content.Load<Texture2D>("endMenu");
            font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(480, 0), null, new Color(255, 255, 255, TransitionAlpha), 1.57f, Vector2.Zero, 1.01f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Your Score: " + score, new Vector2(100, 140), Color.White, 1.57f, Vector2.Zero, 1.7f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        //public void addBestScore()
        //{
        //    Guide.BeginShowKeyboardInput(
        //    PlayerIndex.One,
        //    "congratulations, new high score",
        //    "Insert your name",
        //    "",
        //    new AsyncCallback(OnEndShowKeyboardInput),
        //    null);
        //}

        //private void OnEndShowKeyboardInput(IAsyncResult result)
        //{
        //    string name = Guide.EndShowKeyboardInput(result);
        //    Debug.WriteLine(name);
        //}


    }
}
