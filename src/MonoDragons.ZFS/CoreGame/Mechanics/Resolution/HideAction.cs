using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    class HideAction
    {
        public HideAction()
        {
            Event.Subscribe(EventSubscription.Create<HideSelected>(OnHideChosen, this));
        }

        private void OnHideChosen(HideSelected e)
        {
            EventQueue.Instance.Add(new ActionReadied(() =>
            {
                CurrentData.CurrentCharacter.State.IsHiding = true;
                EventQueue.Instance.Add(new ActionResolved());
            }));
        }
    }
}
