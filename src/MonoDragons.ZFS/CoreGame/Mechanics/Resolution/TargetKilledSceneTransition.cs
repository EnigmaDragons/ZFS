using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public class TargetKilledSceneTransition
    {
        public TargetKilledSceneTransition()
        {
            //TODO: This some trash temp code
            Event.Subscribe<CharacterDeceased>(_ =>
            {
                if (CurrentData.Characters.All(x => !x.State.MustKill || x.State.IsDeceased))
                    Scene.NavigateTo(CurrentData.Characters.First(x => x.State.MustKill).State.NextScene);
            }, this);
        }
    }
}
