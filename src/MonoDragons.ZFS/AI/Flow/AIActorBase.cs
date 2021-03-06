﻿using System;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.AI
{
    public abstract class AIActorBase
    {
        protected Character Char => CurrentData.CurrentCharacter;

        protected void IfAITurn(Action queueAction)
        {
            if (Char.Team.IsIncludedIn(TeamGroup.NeutralsAndEnemies))
                Event.Queue(new AIActionQueued(queueAction));            
        }
    }
}
