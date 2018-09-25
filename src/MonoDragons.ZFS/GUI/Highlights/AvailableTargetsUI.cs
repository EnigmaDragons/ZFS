using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.GUI
{
    public class AvailableTargetsUI : IVisual
    {
        private readonly Dictionary<Point, List<IVisual>> _targetVisuals = new Dictionary<Point, List<IVisual>>();

        public AvailableTargetsUI()
        {
            Event.Subscribe(EventSubscription.Create<ShootSelected>(ShowOptions, this));
            Event.Subscribe(EventSubscription.Create<ActionCancelled>(e => ClearOptions(), this));
            Event.Subscribe(EventSubscription.Create<ActionConfirmed>(e => ClearOptions(), this));
        }

        private void ClearOptions()
        {
            _targetVisuals.Clear();
        }

        private void ShowOptions(ShootSelected e)
        {
            e.AvailableTargets.ForEach(x =>
            {
                _targetVisuals[x.Character.CurrentTile.Position] = new List<IVisual>();
                x.CoverToThem.Covers.SelectMany(c => c.Providers).GroupBy(p => p.Position).ForEach(g =>
                {
                    _targetVisuals[x.Character.CurrentTile.Position].Add(new UiImage { Tint = Color.White.WithAlpha(100), Image = "UI/cover-" + g.Count(), Transform = g.First().Transform });
                    _targetVisuals[x.Character.CurrentTile.Position].Add(new Label { TextColor = UiColors.AvailableTargetsUI_CoverPercentText, Transform = g.First().Transform });
                });
                _targetVisuals[x.Character.CurrentTile.Position].Add(new Label { Text = $"{new HitChanceCalculation(CurrentData.CurrentCharacter.Accuracy, x.CoverToThem.BlockChance, x.Character.Stats.Agility, x.Character.State.IsHiding).Get()}%", Transform = x.Character.CurrentTile.Transform });
                _targetVisuals[x.Character.CurrentTile.Position].Add(new Label { Text = $"{new HitChanceCalculation(x.Character.Accuracy, x.CoverFromThem.BlockChance, CurrentData.CurrentCharacter.Stats.Agility, CurrentData.CurrentCharacter.State.IsHiding).Get()}% ", Transform = CurrentData.CurrentCharacter.CurrentTile.Transform });
            });
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_targetVisuals.ContainsKey(CurrentData.HoveredTile))
                _targetVisuals[CurrentData.HoveredTile].ForEach(x => x.Draw(parentTransform));
        }
    }
}
