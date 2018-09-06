using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.AI
{
    public class AICharacterData
    {
        public Dictionary<Character, Point> SeenEnemies { get; set; } = new Dictionary<Character, Point>();
    }
}
