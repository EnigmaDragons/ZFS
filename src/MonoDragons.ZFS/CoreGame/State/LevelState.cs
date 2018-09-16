using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class LevelState
    {
        public string Name { get; }
        public PrimaryObjective PrimaryObjective { get; }
        public GameMap Map { get; } 
        public CharacterTurns Turns { get; }
        public IReadOnlyList<Character> Characters { get; }
        
        public Character CurrentCharacter => Turns.CurrentCharacter;
        
        public IReadOnlyList<Character> LivingCharacters => Characters.ToList().Where(x => !x.State.IsDeceased).ToList();
        public IEnumerable<Character> Friendlies => FriendliesWhere();
        public IEnumerable<Character> Enemies => LivingCharacters.Where(x => x.Team.IsIncludedIn(TeamGroup.Enemies));
        public IEnumerable<Character> FriendliesWhere(Predicate<Character> wherePredicate = null)
            => LivingCharacters.Where(x => x.Team == Team.Friendly && (wherePredicate?.Invoke(x) ?? true) );
        
        public DictionaryWithDefault<Point, bool> FriendlyPerception { get; set; } = new DictionaryWithDefault<Point, bool>(false);

        public LevelState(string name, GameMap map, IReadOnlyList<Character> chars, CharacterTurns turns, PrimaryObjective primaryObjective)
        {
            Name = name;
            PrimaryObjective = primaryObjective;
            Map = map;
            Characters = chars;
            Turns = turns;
        }
    }
}
