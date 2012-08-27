using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienGameSample;
using Microsoft.Phone.Shell;

namespace attackGame
{
    class MainMenuScreen : MenuScreen
    {

        public MainMenuScreen()
            : base("Main")
        {
            // Create our menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("START GAME");
            MenuEntry highScoreEntry = new MenuEntry("HIGH SCORES");
            MenuEntry optionsEntry = new MenuEntry("OPTIONS");
            MenuEntry exitMenuEntry = new MenuEntry("QUIT");

            // Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(highScoreEntry);
            MenuEntries.Add(optionsEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            (PhoneApplicationService.Current.State[attackGame.GameStateKey] as GameplayHelper).Start();

            ScreenManager.AddScreen(new GameplayScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
            
        }

    }
}
