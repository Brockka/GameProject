using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using GameProject.StateManagement;
using GameProject.Screens;

namespace GameProject
{
    public class GameProjectGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;


        /// <summary>
        /// Constructs the game
        /// </summary>
        public GameProjectGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
            //_screenManager.AddScreen(new SplashScreen(), null);
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent() { }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">the measured game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);    
        }
    }
}
