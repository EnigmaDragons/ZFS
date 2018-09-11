using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;

namespace MonoDragons.Core.Development
{
    public sealed class HoveredElementView : IVisual
    {
        private readonly Point _position;

        private string _elementName = nameof(NoElement);

        public HoveredElementView()
            : this(new Point(1400, 866))
        {
        }

        public HoveredElementView(Point position)
        {
            _position = position;
            Event.SubscribeForever<ActiveElementChanged>(c => { _elementName = c.NewElement.GetType().Name; }, this);
        }

        public void Draw(Transform2 parentTransform)
        {
            var color = DevText.Color;
            var font = DevText.Font;
            UI.DrawTextRight($"Hov: {_elementName}", new Rectangle(_position, new Point(200, DevText.LineHeight)), color, font);
        }
    }
}