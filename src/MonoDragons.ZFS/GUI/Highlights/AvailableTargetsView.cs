using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.GUI
{
    public class AvailableTargetsView : IVisualAutomaton
    {
        private List<IVisual> _visuals = new List<IVisual>();
        private List<IAutomaton> _automata = new List<IAutomaton>();

        public AvailableTargetsView()
        {
            Event.Subscribe(EventSubscription.Create<ShootSelected>(ShowOptions, this));
            Event.Subscribe(EventSubscription.Create<ActionCancelled>(e => ClearOptions(), this));
            Event.Subscribe(EventSubscription.Create<ActionConfirmed>(e => ClearOptions(), this));
        }

        private void ClearOptions()
        {
            _visuals = new List<IVisual>();
            _automata = new List<IAutomaton>();
        }

        private void ShowOptions(ShootSelected e)
        {
            var visuals = new List<IVisual>();
            var automata = new List<IAutomaton>();
            e.AvailableTargets.ForEach(x =>
            {
                visuals.Add(new ColoredRectangle
                {
                    Transform = x.Character.CurrentTile.Transform, 
                    Color = UiColors.AvailableTargetsView_Rectanges
                });
                var anim = new TileRotatingEdgesAnim(x.Character.CurrentTile.Position, UiColors.AvailableTargetsView_TileRotatingEdgesAnim);
                anim.Init();
                visuals.Add(anim);
                automata.Add(anim);
            });
            _visuals = visuals;
            _automata = automata;
        }

        public void Draw(Transform2 parentTransform)
        {
            _visuals.ToList().ForEach(x => x.Draw(parentTransform));
        }

        public void Update(TimeSpan delta)
        {
            _automata.ToList().ForEach(x => x.Update(delta));
        }
    }
}
