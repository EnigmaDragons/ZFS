using System;
using MonoDragons.ZFS.CoreGame;

namespace MonoDragons.ZFS.AI
{
    class AIActionQueued
    {
        public Action Action { get; }
        public TimeSpan Delay { get;  }

        public AIActionQueued(Action action)
            : this(action, CurrentData.FriendlyPerception[CurrentData.CurrentCharacter.CurrentTile.Position] 
                  ? TimeSpan.FromMilliseconds(500) 
                  : TimeSpan.FromMilliseconds(0)) { }

        public AIActionQueued(Action action, TimeSpan delay)
        {
            Action = action;
            Delay = delay;
        }
    }
}
