using System;
using System.Linq;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;

namespace MonoDragons.ZFS.CoreGame
{
    public sealed class DefeatAllEnemies : PrimaryObjective
    {
        public string Description => "Defeat All Enemies";
        
        public void Update(TimeSpan delta)
        {
            if (CurrentData.Enemies.All(x => x.State.IsDeceased))
                Event.Queue(new PrimaryObjectiveCompleted(CurrentData.LevelName));
        }
    }
}
