using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.Tiles;

namespace MonoDragons.ZFS.CoreGame.Calculators
{
    public class VisibilityCalculation
    {
        private readonly Character _character;
        const int calculation = 18;

        public VisibilityCalculation(Character character)
        {
            _character = character;
        }

        public DictionaryWithDefault<Point, bool> Calculate()
        {
            DictionaryWithDefault<Point, bool> canSee = new DictionaryWithDefault<Point, bool>(false);
            var possibleTiles = new PointRadiusCalculation(_character.CurrentTile.Position, calculation).Calculate()
                .Where(x => CurrentData.Map.Exists(x)).ToList();
            possibleTiles
                .Where(x => new ShotCalculation(_character.CurrentTile, CurrentData.Map[x]).CanShoot())
                .ForEach(x => canSee[x] = true);
            canSee.Where(x => CurrentData.Map[x.Key].Cover == Cover.Heavy)
                .ForEach(x =>
                {
                    RecursiveAddHeavyUp(canSee, x.Key, possibleTiles);
                    RecursiveAddHeavyDown(canSee, x.Key, possibleTiles);
                });
            return canSee;
        }

        private void RecursiveAddHeavyUp(DictionaryWithDefault<Point, bool> canSee, Point seenHeavyTile, List<Point> possiblePoints)
        {
            var potentialPoint = new Point(seenHeavyTile.X, seenHeavyTile.Y - 1);
            if (CurrentData.Map.Exists(potentialPoint) && CurrentData.Map[potentialPoint].Cover == Cover.Heavy && possiblePoints.Contains(potentialPoint))
            {
                canSee[potentialPoint] = true;
                RecursiveAddHeavyUp(canSee, potentialPoint, possiblePoints);
            }
        }

        private void RecursiveAddHeavyDown(DictionaryWithDefault<Point, bool> canSee, Point seenHeavyTile, List<Point> possiblePoints)
        {
            var potentialPoint = new Point(seenHeavyTile.X, seenHeavyTile.Y + 1);
            if (CurrentData.Map.Exists(potentialPoint) && CurrentData.Map[potentialPoint].Cover == Cover.Heavy && possiblePoints.Contains(potentialPoint))
            {
                canSee[potentialPoint] = true;
                RecursiveAddHeavyUp(canSee, potentialPoint, possiblePoints);
            }
        }
    }
}
