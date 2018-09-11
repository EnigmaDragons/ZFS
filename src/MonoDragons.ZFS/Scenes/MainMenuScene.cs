using System;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Animations;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.GUI;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.Scenes
{
    public sealed class MainMenuScene : ClickUiScene
    {
        private readonly string _newGameScene;

        public MainMenuScene(string newGameScene)
        {
            _newGameScene = newGameScene;
        }
        
        public override void Init()
        {
            Input.On(Control.Menu, () => Environment.Exit(0));
            Input.On(Control.Start, StartNewGame);
            Input.On(Control.Select, StartNewGame);
            Sound.Music("main-theme", 0.4f).Play();
            Add(new UiImage { Image = "Backgrounds/mainmenu-bg", Transform = new Transform2(new Size2(1920, 1080)) });
            Add(new ColoredRectangle { Color = UiColors.MainMenuScene_Background, Transform = new Transform2(new Size2(1920, 1080)) });
            Add(new UiImage { Image = "UI/title-bg", Transform = new Transform2(new Vector2(UI.OfScreenWidth(0.5f) - 452, 180), new Size2(904, 313)) });
            var button = new TextButton(
                new Rectangle(UI.OfScreenWidth(0.5f) - 150, 700, 300, 50),
                () => {
                    Buttons.PlayClickSound();
                    StartNewGame();
                }, 
                "New Game",
                UiColors.Buttons_Default,
                UiColors.Buttons_Hover,
                UiColors.Buttons_Press
            );
            
            var button2 = new TextButton(
                new Rectangle(UI.OfScreenWidth(0.5f) - 150, 780, 300, 50),
                () => {
                    Buttons.PlayClickSound();
                    CurrentData.Clear();
                    Scene.NavigateTo("Credits");
                }, 
                "Credits",
                UiColors.Buttons_Default,
                UiColors.Buttons_Hover,
                UiColors.Buttons_Press
            );
            
            AddClickable(button);
            AddClickable(button2);
            Add(new ScreenFade {Duration = TimeSpan.FromSeconds(1)}.Started());
        }

        private void StartNewGame()
        {
            CurrentData.Clear();
            Scene.NavigateTo(_newGameScene);
        }

        public override void Dispose() { }
    }
}
