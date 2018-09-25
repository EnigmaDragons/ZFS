using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Development;
using MonoDragons.Core.Errors;
using MonoDragons.Core.Memory;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Engine
{
    public sealed class MonoDragonsGame : Game
    {
        private readonly string _startingViewName;
        private readonly GraphicsDeviceManager _graphics;
        private readonly IErrorHandler _errorHandler;
        private readonly Display _display;
        private readonly GameRoot _root;

        private SpriteBatch _worldSpriteBatch;
        private SpriteBatch _uiSpriteBatch;

        public MonoDragonsGame(string title, string startingViewName, Display display, IErrorHandler errorHandler, GameRoot root)
        {
            Window.Title = title;
            _startingViewName = startingViewName;
            _display = display;
            _errorHandler = errorHandler;
            _root = root;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            HandleEx("Error while Initializing MonoDragons Core engine", () =>
            {
                Perf.Time($"{nameof(MonoDragonsGame)}.Initialize", () =>
                {
                    CurrentGame.Init(this);
                    Resources.Init();
                    CurrentDisplay.Init(_graphics, _display);
                    Window.Position = new Point(
                        (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - CurrentDisplay.GameWidth) / 2,
                        (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - CurrentDisplay.GameHeight) / 2);
                    _uiSpriteBatch = new SpriteBatch(GraphicsDevice);
                    _worldSpriteBatch = new SpriteBatch(GraphicsDevice);
                    World.Init(_worldSpriteBatch);
                    UI.Init(_uiSpriteBatch);
                    _root.Init();
                    base.Initialize();
                });
            });
        }

        protected override void LoadContent() => HandleEx(nameof(LoadContent), () => Scene.NavigateTo(_startingViewName));

        protected override void UnloadContent() => HandleEx(nameof(UnloadContent), () => Content.Unload());

        protected override void Update(GameTime gameTime) => HandleEx("Error in Update Loop", () => _root.Update(gameTime.ElapsedGameTime));

        protected override void Draw(GameTime gameTime)
        {
            HandleEx("Error in Draw Loop", () =>
            {
                _uiSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp);
                _worldSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
                GraphicsDevice.Clear(Color.Black);
                _root.Draw();
                _worldSpriteBatch.End();
                _uiSpriteBatch.End();
            });
        }

        private void HandleEx(string msg, Action execute)
        {
            try { execute(); }
            catch (Exception e) { _errorHandler.Handle(new Exception(msg, e)); }
        }
    }
}
