using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.AI
{
    public class AICharacterDataInitializer : AIActorBase
    {
        private readonly Dictionary<Character, AICharacterData> _characterData;
        private bool _initialized = false;

        public AICharacterDataInitializer(Dictionary<Character, AICharacterData> characterData)
        {
            _characterData = characterData;
            Event.Subscribe<TurnBegun>(_ => Init(), this);
        }

        private void Init()
        {
            if (_initialized)
                return;
            CurrentData.LivingCharacters.Where(x => !x.IsFriendly).ForEach(x => _characterData[x] = new AICharacterData());
            _initialized = true;
        }
    }
}
