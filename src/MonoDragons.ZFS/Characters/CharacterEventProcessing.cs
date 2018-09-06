using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.ActionEvents;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.GUI;

namespace MonoDragons.ZFS.Characters
{
    public sealed class CharacterEventProcessing
    {
        public CharacterEventProcessing()
        {
            Event.Subscribe<OverwatchTilesAvailable>(UpdateOverwatch, this);
            Event.Subscribe<AttackAnimationsFinished>(CheckForDeath, this);
            Event.Subscribe<TilesSeen>(OnTilesSeen, this);
            Event.Subscribe<TilesPerceived>(OnTilesPerceived, this);
            Event.Subscribe<MovementConfirmed>(OnMovementConfirmed, this);
            Event.Subscribe<MoveResolved>(ContinueMoving, this);
            Event.Subscribe<ShotHit>(OnShotHit, this);
            Event.Subscribe<ShotFired>(OnShotFired, this);
            Event.Subscribe<TurnBegun>(OnTurnBegun, this);
        }

        private void OnTurnBegun(TurnBegun e) => e.Character.Notify(e);
        private void ContinueMoving(MoveResolved e) => e.Character.Notify(e);
        private void OnTilesPerceived(TilesPerceived e) => e.Character.Notify(e);
        private void OnTilesSeen(TilesSeen e) => e.Character.Notify(e);
        private void OnShotHit(ShotHit e) => e.Target.Notify(e);
        private void OnShotFired(ShotFired e) => e.Attacker.Notify(e);
        private void CheckForDeath(AttackAnimationsFinished e) => e.Target.Notify(e);
        private void UpdateOverwatch(OverwatchTilesAvailable e) => CurrentData.CurrentCharacter.Notify(e);
        private void OnMovementConfirmed(MovementConfirmed e) => CurrentData.CurrentCharacter.Notify(e);
    }
}