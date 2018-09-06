using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    class MovementOptionsAvailable
    {
        public IReadOnlyList<IReadOnlyList<Point>> AvailableMoves { get; internal set; }
    }
}
