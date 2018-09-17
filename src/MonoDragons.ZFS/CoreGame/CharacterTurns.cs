using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame
{
    public class CharacterTurns : IInitializable
    {
        private int _activeCharacterIndex;
        private readonly List<Character> _characters;
        private bool _isGameOver;
        
        public Character CurrentCharacter { get; private set; }

        public CharacterTurns(IReadOnlyList<Character> characters)
        {
            _characters = characters.OrderByDescending(x => x.Stats.Agility).ToList();
            CurrentCharacter = _characters.First();
            Event.Subscribe(EventSubscription.Create<TurnEnded>(BeginNextTurn, this));
            Event.Subscribe(EventSubscription.Create<CharacterDeceased>(OnCharacterDeath, this));
        }

        public void Init()
        {
            if (_characters.Any(x => x.IsFriendly))
                while (!CurrentCharacter.IsFriendly)
                    Advance();
            PublishTurnBegun();
        }

        private void BeginNextTurn(TurnEnded e)
        {
            Logger.WriteLine($"Entered from {Thread.CurrentThread.ManagedThreadId}");
            if (_isGameOver)
                return;

            Advance();
            while (CurrentCharacter.State.IsDeceased)
                Advance();
            
            PublishTurnBegun();
        }
        
        private void PublishTurnBegun()
        {
            Event.Queue(new TurnBegun { Character = CurrentCharacter });
        }
        
        private void Advance()
        {
            _activeCharacterIndex++;
            if (_activeCharacterIndex == _characters.Count)
                _activeCharacterIndex = 0;
            CurrentCharacter = _characters[_activeCharacterIndex];
        }

        private void OnCharacterDeath(CharacterDeceased e)
        {
            if (CurrentData.Friendlies.All(x => x.State.IsDeceased) || CurrentData.MainCharacter.State.IsDeceased)
            {
                Event.Queue(new GameOver());
                _isGameOver = true;
            }
            else if (CurrentCharacter == e.Victim)
            {
                Event.Queue(new ActionResolved());
            }
        }
    }
}
