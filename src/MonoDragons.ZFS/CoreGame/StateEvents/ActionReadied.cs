using System;

namespace MonoDragons.ZFS.CoreGame.StateEvents
{
    class ActionReadied
    {
        public Action Action { get; }

        public ActionReadied(Action action) => Action = action;
    }
}
