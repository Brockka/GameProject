using Microsoft.Xna.Framework;
using GameProject.StateManagement;

namespace GameProject.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class DeathMenuScreen : MenuScreen
    {
        public DeathMenuScreen(string score) : base("Time survived: " + score)
        {
            var playGameMenuEntry = new MenuEntry("Restart");
            var mainMenuEntry = new MenuEntry("Main Menu");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            mainMenuEntry.Selected += MainMenuEntrySelected;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(mainMenuEntry);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }

        private void MainMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen(), e.PlayerIndex);           
        }
    }
}
