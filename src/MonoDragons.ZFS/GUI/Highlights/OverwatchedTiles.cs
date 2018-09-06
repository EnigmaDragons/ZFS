using System.Collections.Concurrent;
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

namespace MonoDragons.ZFS.GUI
{
    public class OverwatchedTiles : IVisual
    {
        private readonly BlockingCollection<IVisual> _visuals = new BlockingCollection<IVisual>();
        private bool _showOverwatch = false;

        public OverwatchedTiles()
        {
            Event.Subscribe<OverwatchSelected>(x => _showOverwatch = true, this);
            Event.Subscribe<OverwatchTilesAvailable>(x => ShowIfApplicable(x), this);
            Event.Subscribe<ActionConfirmed>(x => HideAndClear(), this);
            Event.Subscribe<ActionCancelled>(x => Hide(), this);
        }

        const float multiplier_percentage_divisor = 2.5f;

        private void ShowIfApplicable(OverwatchTilesAvailable tiles)
        {
            if (!CurrentData.FriendlyPerception[CurrentData.CurrentCharacter.CurrentTile.Position])
                return;
            tiles.OverwatchedTiles.ForEach(x =>
            {
                int percentage = new HitChanceCalculation(CurrentData.CurrentCharacter.Accuracy, x.Value.BlockChance).Get();
                int index = percentage >= 90 ? 4 : (percentage >= 65 ? 3 : (percentage >= 30 ? 2 : 1));
                _visuals.Add(new UiImage
                {
                    Image = "Effects/D_Cover_Overwatched" + index,
                    Transform = CurrentData.Map[x.Key].Transform,
                    Tint = new Color(Color.Wheat, .5f * percentage / 100f),
                });
            });
        }

        private void Hide()
        {
            _showOverwatch = false;
        }

        private void HideAndClear()
        {
            _showOverwatch = false;
            while (_visuals.Count > 0)
                _visuals.Take();
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_showOverwatch)
                _visuals.ToList().ForEach(x => x.Draw(parentTransform));
        }
    }
}
