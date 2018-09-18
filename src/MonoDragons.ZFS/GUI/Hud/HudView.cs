using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
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
            //Add(new TeamTurnHudDecor());
            Add(new CurrentGoalView());
            Add(new ActionOptionsMenu(clickUi));
            Add(new GameOverMenu(clickUi));
            Add(new SwitchWeaponsMenu(clickUi));
            Add(new CharacterStatusView(clickUi, new Point(0.33.VW(), 0.17.VH())));
            var dialogs = new InGameDialogueLayout();
            clickUi.Add(dialogs.Branch);
            Add(dialogs);
        }
    }
}
