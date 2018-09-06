using System;
using System.Linq;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public sealed class FriendlyVisionCalculator : IAutomaton
    {
        private bool _shouldCalc;
        
        public FriendlyVisionCalculator()
        {
            _shouldCalc = true;
            Event.Subscribe<Moved>(e => _shouldCalc = e.Character.Team.IsIncludedIn(TeamGroup.Friendlies), this);
        }

        public void Update(TimeSpan delta)
        {
            if (!_shouldCalc) 
                return;
            
            var friendlyVision = CurrentData.Friendlies
                .SelectMany(x => x.State.SeeableTiles
                    .Where(t => t.Value)
                    .Select(t2 => t2.Key))
                .Distinct()
                .ToList();
                
            CurrentData.Map.Tiles.ForEach(tile =>
            {
                var canSee = friendlyVision.Contains(tile.Position);
                tile.CurrentlyFriendlyVisible = canSee;
                if (canSee)
                    tile.EverSeenByFriendly = true;
            });
        }
    }
}