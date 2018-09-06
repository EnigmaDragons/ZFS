using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    class PassAction
    {
        public PassAction()
        {
            Event.Subscribe<PassSelected>(OnPassSelected, this);
        }

        private void OnPassSelected(PassSelected e)
        {
            EventQueue.Instance.Add(new ActionReadied(() =>
            {
                Event.Publish(new ActionResolved());
            }));
        }
    }
}
