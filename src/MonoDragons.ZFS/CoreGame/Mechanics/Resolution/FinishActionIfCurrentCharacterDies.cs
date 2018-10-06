using System;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public sealed class FinishActionIfCurrentCharacterDies
    {
        private readonly Func<Character> _getCurrentCharacter;

        public FinishActionIfCurrentCharacterDies()
            : this (() => CurrentData.CurrentCharacter) {}
        
        public FinishActionIfCurrentCharacterDies(Func<Character> getCurrentCharacter)
        {
            _getCurrentCharacter = getCurrentCharacter;
            Event.Subscribe(EventSubscription.Create<CharacterDeceased>(OnCharacterDeath, this));
        }

        private void OnCharacterDeath(CharacterDeceased e)
        {
            if (_getCurrentCharacter() == e.Victim)
                Event.Queue(new ActionResolved());
        }
    }
}