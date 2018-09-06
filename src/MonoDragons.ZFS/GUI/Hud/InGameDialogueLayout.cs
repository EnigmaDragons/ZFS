using System;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.CoreGame.StateEvents;

namespace MonoDragons.ZFS.GUI.Hud
{
    public class InGameDialogueLayout : IVisualAutomaton
    {
        private bool _displayingDialogue = false;
        private DialogueView _currentDialogue;

        public ClickUIBranch Branch { get; } = new ClickUIBranch("DialogMaster", 9);

        public InGameDialogueLayout()
        {
            Event.Subscribe<DialogueStarted>(StartDialogue, this);
        }

        public void Update(TimeSpan delta)
        {
            if (!_displayingDialogue)
                return;
            _currentDialogue.Update(delta);
            if (_currentDialogue.IsDone)
            {
                _displayingDialogue = false;
                Branch.Remove(_currentDialogue.Branch);
            }
        }

        public void Draw(Transform2 parentTransform)
        {
            if (_displayingDialogue)
                _currentDialogue.Draw(parentTransform);
        }

        private void StartDialogue(DialogueStarted e)
        {
            _currentDialogue = new DialogueView(e.Dialogs);
            Branch.Add(_currentDialogue.Branch);
            _displayingDialogue = true;
        } 
    }
}
