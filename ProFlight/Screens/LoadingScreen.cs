using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Shell;
using attackGame;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Threading;


namespace attackGame
{
    public class LoadingScreen : GameScreen
    {

        private Thread backgroundThread;
        private bool showPauseScreen;
        public static ContentManager cont;
        public static SpriteBatch sprite;
        public static GraphicsDevice grapDevice;
        bool finish = false;
        GameplayHelper gameplayHelper, cleanHelper;
        //public GameplayHelper gameplayHelper;
        float timer = 0f;
        const float timeToStay = 3f;
        public LoadingScreen(int a, int b)
        {
            this.showPauseScreen = true;
        }

        public LoadingScreen()
        {
            gameplayHelper = gameplayHelper as GameplayHelper;
        }

        void BackgroundLoadContent()
        {

           
            // If we have never created a gameplay helper yet, now is the time. Otherwise, we simply need to reload its content, possibly.
            if (PhoneApplicationService.Current.State.ContainsKey("gameplayHelper"))
            {
                gameplayHelper = attackGame.hel;
                cont = ScreenManager.Game.Content;
                sprite = ScreenManager.SpriteBatch;
                grapDevice = ScreenManager.Game.GraphicsDevice;

            }
            else
            {
                gameplayHelper = new GameplayHelper(ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                    ScreenManager.Game.GraphicsDevice);
                cleanHelper = new GameplayHelper(ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                    ScreenManager.Game.GraphicsDevice);

                if (PhoneApplicationService.Current.State.ContainsKey("newGame"))
                {
                    PhoneApplicationService.Current.State.Remove("newGame");
                }
                PhoneApplicationService.Current.State.Add("newGame", cleanHelper as GameplayHelper);
                //}
                PhoneApplicationService.Current.State.Add("gameplayHelper", gameplayHelper as GameplayHelper);

                cont = ScreenManager.Game.Content;
                sprite = ScreenManager.SpriteBatch;
                grapDevice = ScreenManager.Game.GraphicsDevice;

            }

            if ((ScreenManager.Game as attackGame).ReloadRequired)
            {
                gameplayHelper.InitializeAssets(ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.Game.GraphicsDevice);
                cleanHelper.InitializeAssets(ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.Game.GraphicsDevice);
                gameplayHelper.LoadContent();
                cleanHelper.LoadContent();
            }
        }

        public override void LoadContent()
        {
            if (backgroundThread == null)
            {
                backgroundThread = new Thread(BackgroundLoadContent);
                backgroundThread.Start();
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += elapsed;
            //while(!finish);
            if (backgroundThread != null && backgroundThread.Join(3000))
            {
                
                

                if (showPauseScreen)
                {
                    // TODO: Add pause screen
                }
                else
                {
                    if(timer >= timeToStay)
                    {
                        backgroundThread = null;
                        this.ExitScreen();
                        SplashScreen.drawAnimation = false;
                        ScreenManager.AddScreen(new PhoneMainMenu());
                    }
                    
                }

                ScreenManager.Game.ResetElapsedTime();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
