using System;
using System.Collections.Generic;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    class ActionProposal
    {
        private Queue<Action> _proposed = new Queue<Action>(1);

        public ActionProposal()
        {
            Event.Subscribe<ActionReadied>(Set, this);
            Event.Subscribe<ActionConfirmed>(e =>
            {
                if (_proposed.Any())
                    _proposed.Dequeue().Invoke();
            }, this);
            Event.Subscribe<ActionCancelled>(e => _proposed.Clear(), this);
        }

        private void Set(ActionReadied e)
        {
            _proposed.Clear();
            _proposed.Enqueue(e.Action);
        }
    }
}
