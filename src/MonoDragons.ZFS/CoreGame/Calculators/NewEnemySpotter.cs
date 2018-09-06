using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class NewEnemySpotter
    {
        private readonly DictionaryWithDefault<Character, bool> _hasSeen = new DictionaryWithDefault<Character, bool>(false);

        public NewEnemySpotter()
        {
            Event.Subscribe<MoveResolved>(e => Spot(e.Character), this);
        }

        public void Spot(Character character)
        {
            if (character.Team == Team.Enemy && !_hasSeen[character] 
                && CurrentData.Friendlies.Any(x => CurrentData.FriendlyPerception[character.CurrentTile.Position]))
            {
                _hasSeen[character] = true;
                EventQueue.Instance.Add(new EnemySpotted { Enemy = character });
            }
            else if (character.Team == Team.Friendly)
            {
                CurrentData.LivingCharacters.Where(x => x.Team == Team.Enemy && !_hasSeen[x] && character.State.CanPercieve(x.CurrentTile.Position))
                    .ForEach(x =>
                    {
                        _hasSeen[x] = true;
                        EventQueue.Instance.Add(new EnemySpotted { Enemy = x });
                    });
            }
        }
    }
}
