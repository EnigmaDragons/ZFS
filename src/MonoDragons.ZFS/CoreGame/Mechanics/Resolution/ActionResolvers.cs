using System.Collections.Generic;

namespace MonoDragons.ZFS.CoreGame.Mechanics.Resolution
{
    public static class ActionResolvers
    {
        public static List<object> CreateAll()
        {
            return new List<object>
            {
                new ActionProposal(),
                new HideAction(),
                new RangedAction(),
                new PassAction(),
                new OverwatchAction(),
                new OverwatchResolver()
            };
        }
    }
}
