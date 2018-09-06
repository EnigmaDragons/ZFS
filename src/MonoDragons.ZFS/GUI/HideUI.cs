using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame;
using MonoDragons.ZFS.CoreGame.Mechanics.Events;
using MonoDragons.ZFS.CoreGame.StateEvents;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.GUI
{
    public class HideUI : IVisual
    {
        private readonly UiImage _hideBonusImage = new UiImage { Image = "UI/shield-placeholder-1" };
        private readonly Label _hideBonusLabel = new Label { Text = "+100%", TextColor = UiColors.InGame_Text };
        private bool _hidden = true;

        public HideUI()
        {
            Event.Subscribe<HideSelected>(Show, this);
            Event.Subscribe<ActionCancelled>(_ => Hide(), this);
            Event.Subscribe<ActionConfirmed>(_ => Hide(), this);
        }

        private void Show(HideSelected e)
        {
            _hideBonusImage.Transform = CurrentData.CurrentCharacter.CurrentTile.Transform;
            _hideBonusLabel.Transform = CurrentData.CurrentCharacter.CurrentTile.Transform;
            _hidden = false;
        }

        private void Hide()
        {
            _hidden = true;
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_hidden)
                return;
            _hideBonusImage.Draw(parentTransform);
            _hideBonusLabel.Draw(parentTransform);
        }
    }
}
