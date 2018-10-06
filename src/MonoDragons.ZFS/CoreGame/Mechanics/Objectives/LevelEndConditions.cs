using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class LevelEndConditions
    {
        private readonly GameOverCondition[] _conditions;

        public LevelEndConditions()
            : this(new MainCharacterDied(), new AllFriendliesAreDead()) {}
        
        public LevelEndConditions(params GameOverCondition[] conditions)
        {
            _conditions = conditions;
            Event.Subscribe(EventSubscription.Create<CharacterDeceased>(OnCharacterDeath, this));
        }

        private void OnCharacterDeath(CharacterDeceased e) => 
            _conditions.Where(x => x.IsGameOver())
                .ForEach(c => Event.Publish(new GameOver()));
    }
}