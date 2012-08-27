using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.GamerServices;
using attackGame.Screens;
using Microsoft.Phone.Marketplace;

namespace attackGame
{
    public class GameplayScreen : GameScreen
    {
        //public Boolean isTrial = false;
        //LicenseInformation licenseInfo = new LicenseInformation();
        List<HighScore> sco = new List<HighScore>();
        // Represents the player 
        Player player;
        public Vector2 accelerationInfo;
        // Input Members
        public bool gameOver = false;
        public HighScore Scores;
        public HighScore tempp;
        public List<HighScore> temp;
        public List<HighScore> rezultati;
        string name;
        bool typeFinish;
     
        ISHelper isoHelper;
        SensorReadingEventArgs<AccelerometerReading> accelState;
        Accelerometer Accelerometer;
        Dictionary<string, int> scores;
        // Create a helper instance for handling game logic
        GameplayHelper gameplayHelper;
        bool newGame;

        public GameplayScreen(bool newGame)
        {
            this.newGame = newGame;
            tempp = new HighScore();
            rezultati = new List<HighScore>();
            isoHelper = new ISHelper();
            typeFinish = false;
            temp = new List<HighScore>();
            Scores = new HighScore();
            GameplayHelper.updateGameTime = true;
            //foreach (HighScore kv in temp)
            //{
            //    Debug.WriteLine(kv.player + kv.score);
            //}

            Accelerometer = new Accelerometer();
            if (Accelerometer.State == SensorState.Ready)
            {
                Accelerometer.CurrentValueChanged += (s, e) =>
                {
                    accelState = e;
                };
                Accelerometer.Start();
            }
        }

        public GameplayScreen()
        {

        }

        public override void LoadContent()
        {
            temp = isoHelper.LoadHighScores("scoress.xml");
            //GameplayHelper.spawnTime = 1f;
         
            if (newGame)
            {
                Debug.WriteLine("u load content je");
                gameplayHelper = new GameplayHelper(LoadingScreen.cont, LoadingScreen.sprite, LoadingScreen.grapDevice);
                gameplayHelper.InitializeAssets(ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.Game.GraphicsDevice);
                gameplayHelper.LoadContent();
            }
            else gameplayHelper = (GameplayHelper)PhoneApplicationService.Current.State["gameplayHelper"];

            PhoneApplicationService.Current.State[attackGame.InGameKey] = true;

            base.LoadContent();
        }

        public void addBestScore()
        {
            typeFinish = false;
            Guide.BeginShowKeyboardInput(
            PlayerIndex.One,
            "congratulations, new high score",
            "Insert your name",
            "",
            new AsyncCallback(OnEndShowKeyboardInput),
            null);
        }
        private void OnEndShowKeyboardInput(IAsyncResult result)
        {
            name = Guide.EndShowKeyboardInput(result);
            typeFinish = true;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //if (!gameOver)
            //{
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                List<HighScore> cc = new List<HighScore>();

                if (gameplayHelper.score >= 1000)
                {
                    GameplayHelper.StopMusic();
                    PhoneApplicationService.Current.State[attackGame.InGameKey] = false;
                    ExitScreen();
                    ScreenManager.AddScreen(new DemoScreen());
                }

                if (GameplayHelper.playerHealth <= 0)
                {
                    gameOver = true;
                    
                    GameplayHelper.StopMusic();
                    if (Scores.CheckScores(gameplayHelper.score, temp))
                    {
                        addBestScore();
                        while (name == null && typeFinish == false) ;
                        if (name == null) name = "unknown";
                        temp = Scores.SaveScores(name, gameplayHelper.score, temp);
                        cc = Scores.SortList(temp);
                        isoHelper.SaveHighScores("scoress.xml", cc);
                        ExitScreen();
                        PhoneApplicationService.Current.State[attackGame.InGameKey] = false;
                        PhoneMainMenu.checkSetting = true;
                        ScreenManager.AddScreen(new PhoneMainMenu());
                        
                    }

                    else
                    {
                        PhoneApplicationService.Current.State[attackGame.InGameKey] = false;
                        ExitScreen();
                        ScreenManager.AddScreen(new GameOverScreen(gameplayHelper.score));
                    }
                }
                if (IsActive)
                {
                    gameplayHelper.Update(gameTime, accelerationInfo);
                }
            //}
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            gameplayHelper.Draw(elapsedSeconds);
        }

        public override void HandleInput(InputState input)
        {
            accelerationInfo = accelState == null ? Vector2.Zero :
                new Vector2((float)accelState.SensorReading.Acceleration.X * 2.5f,
                    -(float)accelState.SensorReading.Acceleration.Y * 3.5f);


             Vector3 _accelerationInfo = accelState == null ? Vector3.Zero :
                new Vector3((float)accelState.SensorReading.Acceleration.X,
                    (float)accelState.SensorReading.Acceleration.Y, (float)accelState.SensorReading.Acceleration.Z);
            
            if (input.PauseGame)
            {
                GameplayHelper.isPauseGame = true;
                GameplayHelper.updateGameTime = true;
                PauseCurrentGame();
                GameplayHelper.PauseMusic();
            }
            if (input.DeactivateGame)
            {
             
            }

        }

        public void PauseCurrentGame()
        {
           
            ScreenManager.AddScreen(new PhonePauseScreen());
        }

        private void FinishCurrentGame()
        {
            //
            ExitScreen();
            
        }

        

    }
}
