namespace MonoDragons.ZFS.CoreGame.Mechanics.Events
{
    public sealed class PrimaryObjectiveCompleted
    {
        public string LevelName { get; }

        public PrimaryObjectiveCompleted(string levelName) => LevelName = levelName;
    }
}
