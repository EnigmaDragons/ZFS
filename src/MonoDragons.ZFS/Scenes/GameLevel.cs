﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Animations;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Scenes;
using MonoDragons.ZFS.Characters;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Soundtrack;
using MonoDragons.ZFS.Tiles;
using MonoTiled.Tiled.TmxLoading;

namespace MonoDragons.ZFS.Scenes
{
    public class GameLevel : SimpleScene
    {
        private readonly PrimaryObjective _objective;
        private string MapDir { get; }
        private string MapFileName { get; }
        private Point CameraStartingTile { get; }
        private LevelMusic Music { get; }

        public GameLevel(string mapFileName)
            : this("Maps2", mapFileName) { }
        
        public GameLevel(string mapFileName, LevelMusic music)
            : this("Maps2", mapFileName, music, new DefeatAllEnemies()) { }

        public GameLevel(string mapDir, string mapFileName)
            : this(mapDir, mapFileName, new LevelMusic("corp-amb", "corp-action", "corp-boss", 0.5f, 0.36f, 0.36f), new DefeatAllEnemies()) { }

        private GameLevel(string mapDir, string mapFileName, LevelMusic music, PrimaryObjective objective)
        {
            _objective = objective;
            MapDir = mapDir;
            MapFileName = mapFileName;
            CameraStartingTile = CameraStartingTile;
            Music = music;
            Add(Music);
            Event.Subscribe<MoodChange>(OnMoodChange, this);
        }            

        public override void Init()
        {
            Music.Play(MusicType.Ambient);
            var map = LoadMap();
            CurrentData.Load(new LevelState(MapFileName.Replace(".tmx", ""), map, map.Characters, new CharacterTurns(map.Characters), _objective));
            Add(new TacticsGame(new TurnBasedCombat(map), CameraStartingTile).Initialized());
            Add(new TheSoundGuy());
            Add(new ScreenFade {Duration = TimeSpan.FromSeconds(2)}.Started());
        }

        private void OnMoodChange(MoodChange change)
        {
            if (change.NewMood == Mood.Stealth)
                Music.Play(MusicType.Ambient);
            if (change.NewMood == Mood.Battle)
                Music.Play(MusicType.Action);
            if (change.NewMood == Mood.Boss)
                Music.Play(MusicType.Boss);
        }

        private GameMap LoadMap()
        {
            return Perf.Time("Loaded Map", 
                () => new GameMapFactory().CreateGameMap(
                    new Tmx(CurrentGame.GraphicsDevice, MapDir, MapFileName), 
                    TileData.RenderSize));
        }

        public override void Dispose() { }
    }
}
