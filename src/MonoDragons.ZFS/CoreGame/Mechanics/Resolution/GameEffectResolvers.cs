using System.Collections.Generic;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public class GameEffectResolvers
    {
        public static List<object> CreateAll()
        {
            return new List<object>
            {
                new GainXpOnEnemyDeath(),
                new NavigateToMainMenuOnLevelCompleted()
            };
        }
    }
}
