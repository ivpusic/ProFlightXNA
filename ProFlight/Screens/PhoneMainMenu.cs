using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Shell;
using AlienGameSample;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using attackGame.Screens;
using attackGame.Game_parts;
using Microsoft.Phone.Tasks;

namespace attackGame
{
    class PhoneMainMenu : PhoneMenuScreen
    {
        
        public Song gameMenuMusic;
        public bool inGamee;
        public bool isPlaying = false;
        private List<bool> options = new List<bool>();
        ISOptions iso;
        Texture2D background;
        Player player;
        Vector2 splashPosition;
        Vector2 playerPosition;
        public static bool checkSetting = false;
        Texture2D playerTexture;
        SplashAnimation splashAnimation;
        
        

        ScreenSplash playerr;

        public PhoneMainMenu()
            : base("")
        {

            iso = new ISOptions();
            inGamee = (bool)PhoneApplicationService.Current.State[attackGame.InGameKey];
            Button resume = new Button("", "button_resume");
            resume.Tapped += resume_Tapped;
            //if (PhoneApplicationService.Current.State[attack] == true)
            if(inGamee)
                MenuButtons.Add(resume);
            playerPosition = new Vector2(10, 10);

            Button newGame = new Button("", "button_newGame");
            newGame.Tapped += newGame_Tapped;
            MenuButtons.Add(newGame);

            Button help = new Button("", "button_help");
            help.Tapped += help_Tapped;
            MenuButtons.Add(help);

            Button highScore = new Button("", "button_highScores");
            highScore.Tapped += highScore_Tapped;
            MenuButtons.Add(highScore);

            Button options = new Button("", "button_options");
            options.Tapped += options_Tapped;
            MenuButtons.Add(options);

            Button exitButton = new Button("", "button_quit");
            exitButton.Tapped += exitButton_Tapped;
            MenuButtons.Add(exitButton);
            //Debug.WriteLine("uso je opet u jebeni kontruktor");
            
        }

        public void playMusic()
        {
            Debug.WriteLine(MediaPlayer.State.ToString());
            if(!attackGame.isBackgroundSong)
                MediaPlayer.Play(gameMenuMusic);
        }

        public void stopMusic()
        {


            if (!attackGame.isBackgroundSong)
            {
                MediaPlayer.Stop();
                isPlaying = false;
            }
        }

        public override void LoadContent()
        {
            playerr = new ScreenSplash();
            splashPosition = new Vector2(240, 400);
            splashAnimation = new SplashAnimation();
            options = iso.LoadOptions("options.xml");
            background = ScreenManager.Game.Content.Load<Texture2D>("mainMenu1_demo");
            gameMenuMusic = ScreenManager.Game.Content.Load<Song>("sound/mainMenuMusic");

            if (options[2] == true)
            {
                playMusic();
                isPlaying = true;
                checkSetting = false;
            }
            
        }

        public void CheckSettings()
        {
            options = iso.LoadOptions("options.xml");
            if (options[2] == false && isPlaying) stopMusic();
            if (options[2] == true && !isPlaying) playMusic();
            checkSetting = false;
        }

        public override void HandleInput(InputState input)
        {
            if (input.PauseGame)
            {
                ScreenManager.Game.Exit();
            }
            base.HandleInput(input);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //playerr.Update(gameTime);
            if (!coveredByOtherScreen && checkSetting) CheckSettings();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            //playerr.Draw(spriteBatch);
            spriteBatch.Draw(background, new Vector2(0, 0),
                 new Color(255, 255, 255, TransitionAlpha));
            

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void options_Tapped(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsScreen());
        }

        void help_Tapped(object sender, EventArgs e)
        {
            //ExitScreen();
            ScreenManager.AddScreen(new HelpScreen());
        }

        void resume_Tapped(object sender, EventArgs e)
        {
            this.ExitScreen();
            GameplayHelper.isPauseGame = false;
            GameplayHelper.updateGameTime = true;
            stopMusic();
            GameplayHelper.PlayMusic(GameplayHelper.gameplayMusic);
            //GameplayHelper.ResumeSong();
        }

        /// <summary>
        /// Event handler za "Tapped" event buttona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newGame_Tapped(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new HelpScreen(true));

            stopMusic(); 
        }
        void highScore_Tapped(object sender, EventArgs e)
        {
            //ExitScreen();
            ScreenManager.AddScreen(new HighScoreScreen());
        }
        void exitButton_Tapped(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

    }
}
