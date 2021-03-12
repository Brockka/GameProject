using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {

        private readonly MenuEntry _musicSettings;
        private readonly MenuEntry _soundEffectSettings;

        private static readonly string[] States = { "On", "Off" };
        private static int _currentMusicState;
        private static int _currentEffectState;

        public OptionsMenuScreen() : base("Options")
        {
            _musicSettings = new MenuEntry(string.Empty);
            _soundEffectSettings = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            _musicSettings.Selected += MusicMenuEntrySelected;
            _soundEffectSettings.Selected += SoundEffectMenuEntrySelected;
            back.Selected += OnCancel;

            MenuEntries.Add(_musicSettings);
            MenuEntries.Add(_soundEffectSettings);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _musicSettings.Text = $"Music: {States[_currentMusicState]}";
            _soundEffectSettings.Text = $"Sound Effects: {States[_currentEffectState]}";
        }

        private void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentMusicState == 0)
            {
                MediaPlayer.IsMuted = true;
                _currentMusicState = 1;
            }
            else
            {
                _currentMusicState = 0;
                MediaPlayer.IsMuted = false;
            }
            SetMenuEntryText();
        }

        private void SoundEffectMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (_currentEffectState == 0)
            {
                SoundEffect.MasterVolume = 0.0f;
                _currentEffectState = 1;
            }
            else
            {
                SoundEffect.MasterVolume = 1.0f;
                _currentEffectState = 0;
            }
            SetMenuEntryText();
        }
    }
}
