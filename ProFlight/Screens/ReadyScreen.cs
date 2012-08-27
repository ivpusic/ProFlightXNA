using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows.Threading;
namespace attackGame.Screens
{
    class ReadyScreen : GameScreen
    {
        public SpriteFont font;
        Texture2D bck;
        DispatcherTimer timer;
        ISOptions iso;
        List<bool> options;
        public ReadyScreen()
        {
            iso = new ISOptions();
            options = new List<bool>();
            options = iso.LoadOptions("options.xml");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ExitScreen();
            timer.Stop();
            PhoneMainMenu.checkSetting = true;
            if (options[0] == true)
            {
                GameplayHelper.PlayMusic(GameplayHelper.gameplayMusic);
            }
            ScreenManager.AddScreen(new GameplayScreen(true));
            GameplayHelper.isPauseGame = false;
            GameplayHelper.updateGameTime = true;
        }

        public override void LoadContent()
        {
            bck = ScreenManager.Game.Content.Load<Texture2D>("bgr");
            font = ScreenManager.Game.Content.Load<SpriteFont>("dobarFontJe");
            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(bck, new Vector2(0, 0), null, new Color(255, 255, 255, TransitionAlpha), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, "Ready...", new Vector2(270, 330), Color.White, 1.57f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
