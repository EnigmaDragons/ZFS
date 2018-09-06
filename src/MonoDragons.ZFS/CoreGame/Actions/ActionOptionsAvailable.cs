using System;
using System.Collections.Generic;

namespace MonoDragons.ZFS.CoreGame
{
    class ActionOptionsAvailable
    {
        public Dictionary<ActionType, Action> Options { get; set; } = new Dictionary<ActionType, Action>();
    }
}
