using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Render;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Development
{
    public sealed class DisplayDetailsView : IVisual
    {
        private readonly Vector2 _position;

        public DisplayDetailsView()
            : this(new Point(0, 800)) { }

        public DisplayDetailsView(Point position)
        {
            _position = position.ToVector2();
        }

        public void Draw(Transform2 parentTransform)
        {
            var color = DevText.Color;
            var font = DevText.Font;
            
            UI.DrawText($"Scl: {CurrentDisplay.Scale} - {CurrentDisplay.XScale}, {CurrentDisplay.YScale}", _position, color, font);
            UI.DrawText($"Scr: {GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width}, " +
                        $"{GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height}",
                _position + new Vector2(0, DevText.LineHeight), color, font);
            UI.DrawText($"View: {CurrentDisplay.GameWidth}, {CurrentDisplay.GameHeight}",
                _position + new Vector2(0, DevText.LineHeight * 2), color, font);
        }
    }
}