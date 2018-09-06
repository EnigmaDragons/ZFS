using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public class Moved
    {
        public Point Position { get; set; }
        public Character Character { get; set; }
    }
}
