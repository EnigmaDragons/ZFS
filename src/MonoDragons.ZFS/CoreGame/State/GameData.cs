using System.Collections.Generic;
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class GameData
    {
        public int Currency { get; set; }
        public List<string> ItemNames { get; set; }
        public List<CharacterData> Characters { get; set; }
    }
}
