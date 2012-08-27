using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using AlienGameSample;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;

namespace attackGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class attackGame : Microsoft.Xna.Framework.Game
    {

        SplashScreen backScreen;
        LoadingScreen loadingScreen;
        public const string GameStateKey = "GameState";
        public const string NewGame = "NewGame";
        public const string InGameKey = "InGame";
        public const string KeybackgroundScreen = "bgrScreen";
        public const string KeyloadingScreen = "loadingScreen";
        public static bool isBackgroundSong = false;
        public string test;
        //public string _To { get; set; }
        public string _To;
        public static GameplayHelper hel, tem;
        GameScreen gameScreen;
        /// <summary>
        /// Whether or not asset reloading is required.
        /// </summary>
        public bool ReloadRequired { get; set; }

        /// <summary>
        /// Allows storing the game's gameplay helper.
        /// </summary>
        public static GameplayHelper GameplayHelper { get; set; }

        GraphicsDeviceManager graphics;
        
        ScreenManager screenManager;
        //public GameplayHelper helper, temp;

        public attackGame()
        {
            hel = hel as GameplayHelper;
            _To = "ovo jebeno izgleda da radi";
            graphics = new GraphicsDeviceManager(this);
            PhoneApplicationService phoneAppService = PhoneApplicationService.Current;
            phoneAppService.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            graphics.IsFullScreen = true;
            //Set the Windows Phone screen resolution
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            //
            Content.RootDirectory = "Content";
            backScreen = new SplashScreen();
            loadingScreen = new LoadingScreen(1,2);
            //helper = helper as GameplayHelper;
            //temp = temp as GameplayHelper;
            // Hook up lifecycle events

            PhoneApplicationService.Current.Launching += new EventHandler<LaunchingEventArgs>(Current_Launching);
            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(Current_Activated);
            //PhoneApplicationService.Current.Closing += new EventHandler<ClosingEventArgs>(Current_Closing);
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(Current_Deactivated);

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
            
            //Create a new instance of the Screen Manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            //Debug.WriteLine("konstruktor");
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {
            Debug.WriteLine("deactivating event...");

            if (true == PhoneApplicationService.Current.State.ContainsKey("background"))
            {
                //clear prev value
                PhoneApplicationService.Current.State.Remove("background");
            }

            PhoneApplicationService.Current.State.Add("background", backScreen as SplashScreen);

            if (true == PhoneApplicationService.Current.State.ContainsKey("loading"))
            {
                //clear prev value
                PhoneApplicationService.Current.State.Remove("loading");
            }
            PhoneApplicationService.Current.State.Add("loading", loadingScreen as LoadingScreen);

            hel = (GameplayHelper)PhoneApplicationService.Current.State["gameplayHelper"] as GameplayHelper;
            attackGame.GameplayHelper = hel;

            string SendTo = _To;
            tem = hel;
            PhoneApplicationService.Current.State.Add("Unsaved_To", tem as GameplayHelper);

        }


        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            Debug.WriteLine("activating event...");
            if (PhoneApplicationService.Current.State.ContainsKey("loading"))
            {


            }

            //if (MediaPlayer.State == MediaState.Playing) isBackgroundSong = true;

            backScreen = PhoneApplicationService.Current.State["background"] as SplashScreen;
            loadingScreen = PhoneApplicationService.Current.State["loading"] as LoadingScreen;


            if (PhoneApplicationService.Current.State.ContainsKey("Unsaved_To"))
            {
                tem = hel;
                attackGame.GameplayHelper = hel;
                //tem  = PhoneApplicationService.Current.State["Unsaved_To"] as GameplayHelper;
                PhoneApplicationService.Current.State.Remove("Unsaved_To");
                Debug.WriteLine("ima");
            }


            ReloadRequired = true;

            //PhoneApplicationService.Current.State[InGameKey] = true;

            screenManager = new ScreenManager(this);
           
            // Display the main screen
            screenManager.AddScreen(new SplashScreen());
            screenManager.AddScreen(new LoadingScreen());
        }


        void Current_Launching(object sender, LaunchingEventArgs e)
        {
            ReloadRequired = true;
            Debug.WriteLine("launcing event...");
            PhoneApplicationService.Current.State[InGameKey] = false;
            if (MediaPlayer.State == MediaState.Playing) isBackgroundSong = true;
            //opcije
            
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.FileExists("options.xml"))
            {
                List<bool> obj = new List<bool>();
                obj.Add(true);
                obj.Add(true);
                obj.Add(true);
                IsolatedStorageFileStream stream = storage.CreateFile("options.xml");

                XmlSerializer xml = new XmlSerializer(typeof(List<bool>));
                xml.Serialize(stream, obj);
                GameplayHelper.optionChanged = true;
                stream.Close();
                stream.Dispose();
            }

            //highScores

            IsolatedStorageFile sstorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!sstorage.FileExists("scoress.xml"))
            {
                List<HighScore> scores = new List<HighScore>();
                for(int i = 0; i < 10; i++)
                {
                HighScore h = new HighScore();
                h.Player = "Unknown";
                h.Score = 100;
                scores.Add(h);
                }
                IsolatedStorageFileStream streamm = sstorage.CreateFile("scoress.xml");

                XmlSerializer xmll = new XmlSerializer(typeof(List<HighScore>));
                xmll.Serialize(streamm, scores);

                streamm.Close();
                streamm.Dispose();
            }

            // Display the main screen
            screenManager.AddScreen(new SplashScreen());
            screenManager.AddScreen(new LoadingScreen());
        }

        //public override void Draw(GameTime gameTime)
        //{

        //    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        //    {
                
        //    }
        //}

        /*
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public attackGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        */
    }
}
