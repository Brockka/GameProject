using Microsoft.Xna.Framework;
using GameProject.StateManagement;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class DeathMenuScreen : MenuScreen
    {
        public DeathMenuScreen(string message) : base(message)
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
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen());           
        }

        private void MainMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenuScreen(), e.PlayerIndex);           
        }
    }
}
