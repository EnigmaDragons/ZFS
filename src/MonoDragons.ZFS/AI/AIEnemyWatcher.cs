﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.AI
{
    public class AIEnemyWatcher : AIActorBase
    {
        private readonly Dictionary<Character, AICharacterData> _characterData;
        private AICharacterData Data => _characterData[Char];
        private Mood CurrentMood = Mood.Stealth;

        public AIEnemyWatcher(Dictionary<Character, AICharacterData> characterData)
        {
            _characterData = characterData;
            Event.Subscribe<MoveResolved>(OnMoved, this);
            Event.Subscribe<CharacterDeceased>(OnDeceased, this);
        }

        public void OnMoved(MoveResolved e)
        {
            if (e.Character.Team == Team.Enemy)
            {
                CurrentData.FriendliesWhere(x => e.Character.State.CanPercieve(x.CurrentTile.Position))
                    .ForEach(x => Data.SeenEnemies[x] = x.CurrentTile.Position);
            }
            else
            {
                _characterData.Where(x => x.Key.State.CanPercieve(e.Character.CurrentTile.Position))
                    .ForEach(x => x.Value.SeenEnemies[e.Character] = e.Character.CurrentTile.Position);
            }

            if (CurrentMood == Mood.Stealth && _characterData.Where(x => !x.Key.State.IsDeceased).Any(x => x.Value.SeenEnemies.Any()))
            {
                CurrentMood = Mood.Battle;
                Event.Queue(new MoodChange { NewMood = CurrentMood });
            }
        }

        public void OnDeceased(CharacterDeceased e)
        {
            if (CurrentMood == Mood.Battle && !_characterData.Where(x => !x.Key.State.IsDeceased).Any(x => x.Value.SeenEnemies.Any()))
            {
                CurrentMood = Mood.Stealth;
                Event.Queue(new MoodChange { NewMood = CurrentMood });
            }
            if (e.Victim.IsFriendly)
                _characterData.ForEach(x =>
                {
                    var seenEnemies = new Dictionary<Character, Point>();
                    x.Value.SeenEnemies.Where(enemy => enemy.Key != e.Victim).ForEach(enemy => seenEnemies[enemy.Key] = enemy.Value);
                    x.Value.SeenEnemies = seenEnemies;
                });
        }
    }
}
