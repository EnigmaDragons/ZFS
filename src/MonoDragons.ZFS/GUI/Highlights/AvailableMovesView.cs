using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Themes;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.GUI
{
    class AvailableMovesView : IVisual
    {
        private readonly GameMap _map;
        private List<IVisual> _visuals = new List<IVisual>();

        public AvailableMovesView(GameMap map)
        {
            _map = map;
            Event.Subscribe(EventSubscription.Create<MovementOptionsAvailable>(ShowOptions, this));
            Event.Subscribe(EventSubscription.Create<MovementConfirmed>(OnMovementConfirmed, this));
        }
        
        private void OnMovementConfirmed(MovementConfirmed e)
        {
            _visuals = new List<IVisual>();
        }

        private void ShowOptions(MovementOptionsAvailable e)
        {
            if (!CurrentData.FriendlyPerception[CurrentData.CurrentCharacter.CurrentTile.Position])
                return;
            var visuals = new List<IVisual>();
            e.AvailableMoves.ForEach(x =>
            {
                visuals.Add(new UiImage
                {
                    Image = "Effects/Cover_Gray",
                    Transform = CurrentData.Map.TileToWorldTransform(x.Last()).WithSize(TileData.RenderSize),
                    Tint = UiColors.AvailableMovesView_Rectangles
                });
            });
            _visuals = visuals;
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ToList().ForEach(x => x.Draw(parentTransform));
        }
    }
}
