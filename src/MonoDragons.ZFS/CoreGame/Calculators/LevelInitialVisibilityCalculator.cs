namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public sealed class LevelInitialVisibilityCalculator : IInitializable
    {
        private readonly VisibilityCalculator _visibility;
        private readonly PerceptionCalculator _perception;
        private readonly FriendlyPerceptionUpdater _friendlyPerception;

        public LevelInitialVisibilityCalculator(VisibilityCalculator visibility,
            PerceptionCalculator perception,
            FriendlyPerceptionUpdater friendlyPerception)
        {
            _visibility = visibility;
            _perception = perception;
            _friendlyPerception = friendlyPerception;
        }
        
        public void Init()
        {       
            CurrentData.Characters.ForEach(x =>
            {
                _visibility.UpdateSight(x);
                _perception.UpdatePerception(x);
            });
            _friendlyPerception.UpdatePerception();
        }
    }
}