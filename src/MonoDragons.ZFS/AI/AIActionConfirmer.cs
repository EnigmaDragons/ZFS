using System;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.AI
{
    class AIActionConfirmer : AIActorBase
    {
        public AIActionConfirmer()
        {
            Event.Subscribe<ActionReadied>(e => IfAITurn(() => Event.Publish(new AIActionQueued(
                () => Event.Publish(new ActionConfirmed()),
                CurrentData.FriendlyPerception[CurrentData.CurrentCharacter.CurrentTile.Position]
                    ? TimeSpan.FromSeconds(1.0f)
                    : TimeSpan.FromMilliseconds(0)))), this);
        }
    }
}
