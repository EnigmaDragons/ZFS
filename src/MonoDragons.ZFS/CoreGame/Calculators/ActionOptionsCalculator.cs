using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.EventSystem;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    class ActionOptionsCalculator
    {
        public ActionOptionsCalculator()
        {
            Event.Subscribe<RangedTargetsAvailable>(SendActionOptionsAvailable, this);
        }

        private void SendActionOptionsAvailable(RangedTargetsAvailable e)
        {
            var options = new Dictionary<ActionType, Action>();
            if (e.Targets.Any())
                options[ActionType.Shoot] = () => Select(new ShootSelected { AvailableTargets = e.Targets });
            if (CanHide(CurrentData.CurrentCharacter))
                options[ActionType.Hide] = () => Select(new HideSelected());
            if (CurrentData.CurrentCharacter.Gear.EquippedWeapon.IsRanged)
                options[ActionType.Overwatch] = () => Select(new OverwatchSelected());
            options[ActionType.Pass] = () => Select(new PassSelected());

            Event.Queue(new ActionOptionsAvailable { Options = options });
        }

        private bool CanHide(Character character)
        {
            var point = character.CurrentTile.Position;
            var directions = new List<Point>
            {
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X, point.Y + 1)
            };
            return directions.Any(x => CurrentData.Map.Exists(x) && CurrentData.Map[x].Cover > Cover.None);
        }
        
        private void Select(object selection)
        {
            Event.Queue(new ActionSelected(selection.GetType().Name.Replace("Selected", "")));
            Event.Queue(selection);
        }
    }
}
