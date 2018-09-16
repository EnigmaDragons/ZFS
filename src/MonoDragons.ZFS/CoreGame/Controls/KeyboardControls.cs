using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Inputs;
using MonoDragons.ZFS.GUI;

namespace MonoDragons.ZFS.CoreGame.Controls
{
    public sealed class KeyboardControls
    {
        public KeyboardControls()
        {
            Input.On(Control.Character, () => Event.Queue(new ToggleCharacterStatusViewRequested(CurrentData.CurrentCharacter)));
        }
    }
}
