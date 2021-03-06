﻿using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;
using MonoDragons.ZFS.GUI;

namespace MonoDragons.ZFS.CoreGame
{
    public class GameDrawMaster : IVisual
    {
        private readonly TileFXCollection _fx = new TileFXCollection();

        const int Floors = 0;
        const int Walls = 1;
        const int UnderChar1 = 2;
        const int UnderChar2 = 3;
        const int OverChar1 = 4;
        const int OverChar2 = 5;
        private const int Shadows = 10;
        private const int FogOfWar = 99;

        static readonly Color MultiplyColor = new Color(150, 210, 255, 255);

        public void Draw(Transform2 parentTransform)
        {
            var chars = CurrentData.Characters
                .OrderBy(x => x.CurrentTile.Position.X)
                .ThenBy(x => x.CurrentTile.Position.Y).ToList();
            Perf.Time("Drew Walls + Floors", () => CurrentData.Map.Tiles.ForEach(x =>
            {
                x.Draw(Floors, parentTransform, MultiplyColor);
                x.Draw(Walls, parentTransform, MultiplyColor);
            }));
            Perf.Time("Drew Highlights", () => CurrentData.Highlights.Draw(parentTransform));
            Perf.Time("Drew Under Char Objects", () => CurrentData.Map.Tiles.ForEach(x =>
            {
                x.Draw(UnderChar1, parentTransform, MultiplyColor);
                x.Draw(UnderChar2, parentTransform, MultiplyColor);
            }));
            Perf.Time("Drew Characters", () => chars.ForEach(x => x.Draw(parentTransform)));
            Perf.Time("Drew Over Char Objects", () => CurrentData.Map.Tiles.ForEach(x =>
            {
                x.Draw(OverChar1, parentTransform, MultiplyColor);
                x.Draw(OverChar2, parentTransform, MultiplyColor);
                x.Draw(Shadows, parentTransform, MultiplyColor);
                x.Draw(FogOfWar, parentTransform);
            }));
            Perf.Time("Drew High Highlights", () => CurrentData.HighHighlights.Draw(parentTransform));
            Perf.Time("Drew PostFX", () => CurrentData.Map.Tiles.ForEach(x => _fx.Draw(parentTransform, x)));
            Perf.Time("Drew Char UI", () => chars.ForEach(x => x.DrawUI(parentTransform)));
        }
    }
}
