namespace MonoDragons.ZFS.CoreGame
{
    public sealed class MainCharacterDied : GameOverCondition
    {
        public string Description => $"{CurrentData.MainCharacter.Stats.Name} is dead!";
        public bool IsGameOver() => CurrentData.MainCharacter.State.IsDeceased;
    }
}