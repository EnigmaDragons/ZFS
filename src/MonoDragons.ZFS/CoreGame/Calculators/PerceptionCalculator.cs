using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class PerceptionCalculator
    {
        public PerceptionCalculator()
        {
            Event.Subscribe<Moved>(e => UpdatePerception(e.Character), this);
        }

        public void UpdatePerception(Character character)
        {
            EventQueue.Instance.Add(new TilesPerceived
            {
                Character = character,
                Tiles = new PointRadiusCalculation(character.CurrentTile.Position, character.Stats.Perception).Calculate()
                    .Where(x => CurrentData.Map.Exists(x)).ToList() 
            });
        }
    }
}
