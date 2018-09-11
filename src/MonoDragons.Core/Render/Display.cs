using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Inputs;

namespace MonoDragons.Core.Render
{
    public class Display
    {
        public int GameWidth { get; }
        public int GameHeight { get; }
        
        public bool UseFullscreen { get; private set; }
        private int ActualWidth { get; set; }
        private int ActualHeight { get; set; }
        public float Scale { get; private set; }
        public float XScale { get; private set; }
        public float YScale { get; private set; }
        private GraphicsDeviceManager _graphicsDeviceManager;

        public Display(int width, int height, bool useFullscreen, float scale = 1)
        {
            UseFullscreen = useFullscreen;
            ActualWidth = width;
            ActualHeight = height;
            GameWidth = width;
            GameHeight = height;
            Scale = scale;
        }

        public void Apply(GraphicsDeviceManager deviceManager)
        {
            _graphicsDeviceManager = deviceManager;
            Update();
        }

        public void ToggleFullscreen()
        {
            UseFullscreen = !UseFullscreen;
            Update();
        }

        private void Update()
        {
            var w = UseFullscreen
                ? GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)GameWidth
                : 1.0f;
            var h = UseFullscreen
                ? GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)GameHeight
                : 1.0f;

            XScale = w;
            YScale = h;
            var newScaleModifier = Math.Min(h, w);
            Scale = newScaleModifier;
            ActualWidth = Convert.ToInt32(GameWidth * newScaleModifier);
            ActualHeight = Convert.ToInt32(GameHeight * newScaleModifier);

            _graphicsDeviceManager.PreferredBackBufferWidth = ActualWidth;
            _graphicsDeviceManager.PreferredBackBufferHeight = ActualHeight;
            _graphicsDeviceManager.IsFullScreen = UseFullscreen;
            _graphicsDeviceManager.ApplyChanges();
        }
    }
}
