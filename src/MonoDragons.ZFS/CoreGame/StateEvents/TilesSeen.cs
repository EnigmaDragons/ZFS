using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public class TilesSeen
    {
        public Character Character { get; set; }
        public DictionaryWithDefault<Point, bool> SeeableTiles { get; set; }
    }
}
