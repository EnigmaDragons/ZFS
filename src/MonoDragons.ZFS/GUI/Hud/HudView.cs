using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.Scenes;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.GUI.Hud;
using MonoDragons.ZFS.GUI.Menus;
using MonoDragons.ZFS.GUI.Views;

namespace MonoDragons.ZFS.GUI
{
    class HudView : GameObjContainer
    {
        public HudView(ClickUI clickUi) : base(true)
        {
            Add(new InGameMenu(clickUi));
            Add(new ActionConfirmMenu(clickUi));
            Add(new EquippedWeaponView(new Point(UI.OfScreenWidth(0.834f), UI.OfScreenHeight(0.86f))));
            Add(new CurrentCharacterView(new Point(UI.OfScreenWidth(0.01f), UI.OfScreenHeight(0.86f))));
            Add(new AttackPreview());
            Add(new TeamTurnHudDecor());
            Add(new ActionOptionsMenu(clickUi));
            Add(new GameOverMenu(clickUi));
            Add(new SwitchWeaponsMenu(clickUi));
            Add(new CharacterStatusView(new Point(UI.OfScreenWidth(0.33f), UI.OfScreenHeight(0.17f))));
            var dialogs = new InGameDialogueLayout();
            clickUi.Add(dialogs.Branch);
            Add(dialogs);
        }
    }
}
