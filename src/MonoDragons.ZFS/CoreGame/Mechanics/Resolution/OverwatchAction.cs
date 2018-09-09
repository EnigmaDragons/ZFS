using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.Calculators;
using MonoDragons.ZFS.CoreGame.Mechanics.Covors;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    class OverwatchAction
    {
        public OverwatchAction()
        {
            Event.Subscribe(EventSubscription.Create<OverwatchSelected>(OnOverwatchSelected, this));
        }

        private void OnOverwatchSelected(OverwatchSelected e)
        {
            if (!CurrentData.CurrentCharacter.State.OverwatchedTiles.Any())
            {
                var overwatchedTiles = new Dictionary<Point, ShotCoverInfo>();
                var tiles = ValidTiles(new PointRadiusCalculation(CurrentData.CurrentCharacter.CurrentTile.Position, CurrentData.CurrentCharacter.Gear.EquippedWeapon.AsRanged().Range).Calculate());
                tiles.ForEach(x =>
                {
                    var shot = new ShotCalculation(CurrentData.CurrentCharacter.CurrentTile, CurrentData.Map[x]).GetBestShot();
                    if (new HitChanceCalculation(CurrentData.CurrentCharacter.Accuracy, shot.BlockChance).Get() > 0)
                        overwatchedTiles[x] = shot;
                });
                Event.Queue(new OverwatchTilesAvailable { OverwatchedTiles = overwatchedTiles });
            }
            Event.Queue(new ActionReadied(() =>
            {
                CurrentData.CurrentCharacter.State.IsOverwatching = true;
                Event.Queue(new ActionResolved());
            }));
        }

        private List<Point> ValidTiles(List<Point> points)
        {
            return points.Where(x => CurrentData.Map.Exists(x) && CurrentData.Map[x].IsWalkable).ToList();
        }
    }
}
