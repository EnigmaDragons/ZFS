using System;
using System.Collections.Generic;
using MonoDragons.Core.Engine;

namespace MonoDragons.Core.EventSystem
{
    public sealed class EventQueue : IAutomaton
    {
        private List<object> _eventsToPublish = new List<object>();

        public void Add(object evt)
        {
            _eventsToPublish.Add(evt);
        }

        public void Update(TimeSpan delta)
        {
            var events = _eventsToPublish;
            _eventsToPublish = new List<object>();
            events.ForEach(Event.Publish);
        }
    }
}
