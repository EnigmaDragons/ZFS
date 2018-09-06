using System;

namespace MonoDragons.ZFS.CoreGame
{
    public class ShotConfirmed
    {
        public ShotProposed Proposed { get; set; }
        public Action OnFinished { get; set; }
    }
}
