﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.Characters.Prefabs;
using MonoDragons.ZFS.GUI;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame
{
    internal static class CurrentData
    {
        private static GameData GameData { get; set; }
        private static MustInit<LevelState> LevelState { get; } = new MustInit<LevelState>(nameof(LevelState));
        private static LevelState Level => LevelState.Get();

        internal static string LevelName => Level.Name;
        internal static GameMap Map => Level.Map;
        internal static PrimaryObjective PrimaryObjective => Level.PrimaryObjective;
        internal static Character CurrentCharacter => Level.CurrentCharacter;
        internal static IEnumerable<Character> Characters => Level.Characters;
        internal static IEnumerable<Character> LivingCharacters => Characters.Where(x => !x.State.IsDeceased);
        internal static IEnumerable<Character> FriendliesWhere(Predicate<Character> condition) => Level.FriendliesWhere(condition);
        internal static IEnumerable<Character> Friendlies => Level.Friendlies;
        internal static IEnumerable<Character> Enemies => Level.Enemies;

        internal static DictionaryWithDefault<Point, bool> FriendlyPerception
        {
            get => Level.FriendlyPerception;
            set => Level.FriendlyPerception = value;
        }

        internal static Highlights Highlights { get; set; }
        internal static HighHighlights HighHighlights { get; set; }
        internal static Point HoveredTile { get; set; } = new Point(0, 0);

        internal static CharacterClass MainCharClass { get; set; } = new Leader();
        internal static Character MainCharacter => Level.FriendliesWhere(x => x.Stats.Name.Equals(MainChar.Name)).Single();
        
        internal static void Load(LevelState state)
        {
            LevelState.Init(state);
        }
        
        internal static void Clear()
        {
            GameData = null;
            LevelState.Clear();
            Highlights = null;
            HighHighlights = null;
            HoveredTile = Point.Zero;
        }

        internal static void InitLevel()
        {
            Level.Turns.Init();
        }
    }
}
