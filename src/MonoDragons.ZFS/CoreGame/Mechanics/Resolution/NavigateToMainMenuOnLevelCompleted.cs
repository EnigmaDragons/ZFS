using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public sealed class NavigateToMainMenuOnLevelCompleted
    {
        public NavigateToMainMenuOnLevelCompleted()
        {
            Event.Subscribe<PrimaryObjectiveCompleted>(e => Scene.NavigateTo("MainMenu"), this);
        }
    }
}
