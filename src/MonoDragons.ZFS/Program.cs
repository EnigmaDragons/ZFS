using System;
using System.Windows.Forms;
using MonoDragons.Core.AudioSystem;
using MonoDragons.Core.Development;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Errors;
using MonoDragons.Core.Inputs;
using MonoDragons.Core.Memory;
using MonoDragons.Core.Physics;
using MonoDragons.Core.Render;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.Text;
using MonoDragons.ZFS.Scenes;
using MonoDragons.ZFS.Soundtrack;
using Control = MonoDragons.Core.Inputs.Control;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MonoDragons.ZFS
{
    public static class Program
    {
        static AppDetails AppDetails = new AppDetails("MonoDragons.ZFS", "0.1", Environment.OSVersion.VersionString);
#if DEBUG
        static IErrorHandler ErrorHandler = new MessageBoxErrorHandler();
#else
        static IErrorHandler ErrorHandler = new ReportErrorHandler(AppDetails, "https://hk86vytqs1.execute-api.us-west-2.amazonaws.com/GameMetrics/ReportCrashDetail");
#endif

        static Program()
        {
            DefaultFont.Header = "Fonts/24";
            DefaultFont.Large = "Fonts/18";
            DefaultFont.Medium = "Fonts/14";
            DefaultFont.Body = "Fonts/14";
        }
        
        [STAThread]
        static void Main()
        {
#if DEBUG
            RunGame("DarkAlley");
#else
            RunGame("Logo");
#endif
        }

        private static SceneFactory CreateSceneFactory()
        {
            return new SceneFactory(new Map<string, Func<IScene>>
            {
                { "Credits", () => new CreditsScene() },
                { "Logo", () => new LogoScene("MainMenu") },
                { "MainMenu", () => new MainMenuScene("Intro") },
                { "Intro", () => new IntroCutscene("CharacterCreation") },
                { "CharacterCreation", () => new LegacyCharacterCreationScene("DarkAlley") },
                { "SampleLevel", () => new GameLevel("SampleCorporate.tmx") },
                { "ShootingRange", () => new GameLevel("TestFogOfWar.tmx") },
                { "DarkAlley", () => new GameLevel("DarkAlley.tmx", new LevelMusic("alley-amb", "alley-action-loop", "alley-action-loop", 0.6f, 0.34f, 0.34f)) },
                { "SpawnTest", () => new GameLevel("SpawnTest.tmx") },
                { "FinalFloor", () => new GameLevel("FinalFloor.tmx") },
            });
        }

        private static void RunGame(string sceneName)
        {
            try
            {
                using (var game = Perf.Time("Startup", 
                    () => new MonoDragonsGame(AppDetails.Name, sceneName, new Display(1600, 900, false), ErrorHandler, CreateGameRoot())))
                        game.Run();
            }
            catch(Exception e)
            {
                ErrorHandler.Handle(e);
                MessageBox.Show($"Your game has crashed, probably due to hacking by ZantoCorp.\n\n" +
                                                     $"The Fatal Error has been automatically reported.\n\n" +
                                                     $"Thank you for helping us with Quality Assurance!\n\n" +
                                                     $"Credits have been automatically deposited into your bank account.\n\n" +
                                                     $"Error: '{e.Message}'\n" +
                                                     $"StackTrace: {e.StackTrace}");
            }
        }

        private static IVisualAutomaton Tools()
        {
#if DEBUG
            return new DebugToolsLayout();
#else
            return new Dummy();
#endif
        }

        private static GameRoot CreateGameRoot()
        {
            var objs = new object[]
            {
                SetupInGameKeyboardController(),
                SetupPermanentController(),
                SetupScene(),
                Tools(),
            };
            return new GameRoot(objs);
        }
        
        private static IScene SetupScene()
        {
            var currentScene = new CurrentScene();
                Scene.Init(new CurrentSceneNavigation(currentScene, CreateSceneFactory(),
                    Input.ClearTransientBindings,
                    Resources.Unload, 
                    AudioPlayer.Instance.StopAll));
                return new HideViewportExternals(currentScene);
        }

        private static IController SetupPermanentController()
        {
            var ctrl = new KeyboardController(new Map<Keys, HorizontalDirection>(), new Map<Keys, VerticalDirection>(), new Map<Keys, Control>
            {
                {Keys.OemTilde, Control.Exit},
                {Keys.F10, Control.Fullscreen},
            });
            Input.SetPermanentController(ctrl);
            Input.OnForever(Control.Exit, () => Environment.Exit(0));
            Input.OnForever(Control.Fullscreen, () => CurrentDisplay.Display.ToggleFullscreen());
            return ctrl;
        }
        
        private static IController SetupInGameKeyboardController()
        {
            var ctrl = new KeyboardController(new Map<Keys, Control>
            {
                { Keys.Space, Control.Select },
                { Keys.Enter, Control.Start },
                { Keys.Escape, Control.Menu },
                { Keys.V, Control.A },
                { Keys.O, Control.X }
            });
            Input.SetController(ctrl);
            return ctrl;
        }
    }
}
