﻿using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.ZFS.Characters.Prefabs;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.Characters
{
    class CharacterSpawning
    {
        public List<Character> GetCharacters(GameMap map)
        {
            var chars = map.Tiles.Where(x => !x.SpawnCharacter.Equals("None"))
                .Select(t =>
                {
                    if (t.SpawnCharacter.Equals("main", StringComparison.InvariantCultureIgnoreCase))
                        return new MainChar(CurrentData.MainCharClass).Initialized(map, t);
                    if (t.SpawnCharacter.Equals("sidechick", StringComparison.InvariantCultureIgnoreCase))
                        return new Sidechick().Initialized(map, t);
                    if (t.SpawnCharacter.Equals("corpsec1", StringComparison.InvariantCultureIgnoreCase))
                        return new CorpSec1().Initialized(map, t);
                    if (t.SpawnCharacter.Equals("corpsec2", StringComparison.InvariantCultureIgnoreCase))
                        return new CorpSec2(t.MustKill).Initialized(map, t);
                    if (t.SpawnCharacter.Equals("corpsec3", StringComparison.InvariantCultureIgnoreCase))
                        return new CorpSec3(t.MustKill).Initialized(map, t);
                    Logger.WriteLine($"Unknown SpawnCharacter '{t.SpawnCharacter}'");
                    return new CorpSec1().Initialized(map, t);
                }).ToList();

            return chars;
        }
    }
}
