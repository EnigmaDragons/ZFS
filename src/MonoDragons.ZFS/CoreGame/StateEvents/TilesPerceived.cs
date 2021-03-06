﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    public class TilesPerceived
    {
        public Character Character { get; set; }
        public List<Point> Tiles { get; set; }
    }
}
