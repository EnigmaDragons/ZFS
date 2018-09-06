using System;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;

namespace MonoDragons.ZFS.AI
{
    class AIThinkingDelay : IAutomaton
    {
        private TimerTask _timer;

        public AIThinkingDelay()
        {
            Event.Subscribe<AIActionQueued>(e => _timer = new TimerTask(e.Action, e.Delay.TotalMilliseconds, false), this);
        }

        public void Update(TimeSpan delta)
        {
            _timer?.Update(delta);
        }
    }
}
