using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public class MovementConfirmed
    {
        public IReadOnlyList<Point> Path { get; }

        public MovementConfirmed(IReadOnlyList<Point> path)
        {
            if (path.Count == 0)
                throw new InvalidOperationException("All Movement Paths must have an ending");
            Path = path;
        }
    }
}
