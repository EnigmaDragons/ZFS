using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Render;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Scenes
{
    public sealed class PlaceholderScene : IScene
    {
        public void Init()
        {
        }

        public void Update(TimeSpan delta)
        {
        }

        public void Draw(Transform2 parentTransform)
        {
            UI.DrawTextCentered("Placeholder", CurrentDisplay.FullScreenRectangle, Color.White);
        }

        public void Dispose()
        {
        }
    }
}
