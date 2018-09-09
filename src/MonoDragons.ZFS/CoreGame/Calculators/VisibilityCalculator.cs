using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class VisibilityCalculator
    {
        public VisibilityCalculator()
        {
            Event.Subscribe(EventSubscription.Create<Moved>(e => UpdateSight(e.Character), this));
        }

        public void UpdateSight(Character character)
        {
            Event.Queue(new TilesSeen { Character = character, SeeableTiles = new VisibilityCalculation(character).Calculate() });
        }
    }
}
