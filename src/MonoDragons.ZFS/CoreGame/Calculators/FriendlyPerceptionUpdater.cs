using Microsoft.Xna.Framework;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class FriendlyPerceptionUpdater
    {
        public FriendlyPerceptionUpdater()
        {
            Event.Subscribe<TurnEnded>(_ => UpdatePerception(), this);
        }

        public void UpdatePerception()
        {
            var friendlyPerception = new DictionaryWithDefault<Point, bool>(false);
            CurrentData.Friendlies.ForEach(friendly =>
            {
                friendly.State.SeeableTiles.ForEach(tile => friendlyPerception[tile.Key] = true);
                friendly.State.PerceivedTiles.ForEach(tile => friendlyPerception[tile.Key] = true);
            });
            CurrentData.FriendlyPerception = friendlyPerception;
        }
    }
}
