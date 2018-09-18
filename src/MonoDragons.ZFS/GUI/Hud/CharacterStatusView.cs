using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoDragons.Core.Engine;
using MonoDragons.Core.EventSystem;
using MonoDragons.Core.Physics;
using MonoDragons.Core.UserInterface;
using MonoDragons.ZFS.Themes;

namespace MonoDragons.ZFS.GUI.Views
{
    public sealed class CharacterStatusView : IVisual
    {
        private static readonly int _viewWidth = 0.34.VW();
        private static readonly int _viewHeight = 0.66.VH();
        
        private readonly ClickUIBranch _branch = new ClickUIBranch(nameof(CharacterStatusView), 8);
        private readonly IReadOnlyList<IVisual> _visuals;
        private readonly ClickUI _clickUi;

        private readonly UiImage _face;
        private readonly Label _level;
        private readonly Label _name;
        private readonly Label _maxHp;
        private readonly Label _movement;
        private readonly Label _accuracy;
        private readonly Label _guts;
        private readonly Label _agility;
        private readonly Label _perception;

        private bool _shouldShow;

        public CharacterStatusView(ClickUI clickUi, Point position)
        {
            _clickUi = clickUi;
            var visuals = new List<IVisual>();            
            visuals.Add(new UiImage
            {
                Image = "UI/menu-tall-panel.png", Tint = 220.Alpha(),
                Transform = new Transform2(new Rectangle(position.X, position.Y, _viewWidth, _viewHeight))
            });

            _face = visuals.Added(new UiImage { Transform = new Transform2(new Rectangle(position.X + 50, position.Y + 50, 160, 160)) });
            _name = visuals.Added(new Label
            {
                Font = GuiFonts.Large,
                Transform = new Transform2(new Rectangle(position.X + 20, position.Y + 0.020.VH(), _viewWidth, 50)),
                TextColor = UiColors.InGame_Text
            });
            _level = visuals.Added(new Label
            {
                Font = GuiFonts.Large,
                Transform = new Transform2(new Rectangle(position.X + 10, position.Y + 0.020.VH() + 200, _viewWidth / 2, 50)),
                TextColor = UiColors.InGame_Text
            });
            
            var index = -1;
            _maxHp = visuals.Added(StatLabel(position, ++index));
            _movement = visuals.Added(StatLabel(position, ++index));
            _accuracy = visuals.Added(StatLabel(position, ++index));
            _guts = visuals.Added(StatLabel(position, ++index));
            _agility = visuals.Added(StatLabel(position, ++index));
            _perception = visuals.Added(StatLabel(position, ++index));
            
            var dismissButton = Buttons.Text(new Buttons.MenuContext { Width = _viewWidth, FirstButtonYOffset = 520, X = position.X, Y = position.Y }, 
                0, "Dismiss", () => _shouldShow = false, () => true);
            _branch.Add(dismissButton);
            visuals.Add(dismissButton);
            
            _visuals = visuals;
            Event.Subscribe<ToggleCharacterStatusViewRequested>(UpdateDisplayedCharacter, this);
        }

        private Label StatLabel(Point position, int index)
        {
            return new Label
            {
                Font = GuiFonts.Medium,
                Transform = new Transform2(new Rectangle(position.X + 10,
                    position.Y + 0.02.VH() + 260 + (index) * 40, _viewWidth / 2, 30)),
                TextColor = UiColors.InGame_Text
            };
        }
        
        private void UpdateDisplayedCharacter(ToggleCharacterStatusViewRequested e)
        {
            if (_shouldShow && _name.Text.Equals(e.Character.Stats.Name))
            {
                Hide();
                return;
            }

            Event.Publish(new SubviewRequested());
            var c = e.Character;
            _face.Image = c.FaceImage;
            _level.Text = $"Level {c.Stats.Level}";
            _name.Text = c.Stats.Name;
            _maxHp.Text = $"HP: {c.Stats.HP}";
            _movement.Text = $"MOV: {c.Stats.Movement}";
            _accuracy.Text = $"ACC: {c.Stats.Accuracy}";
            _guts.Text = $"GUT: {c.Stats.Guts}";
            _agility.Text = $"AGI: {c.Stats.Agility}";
            _perception.Text = $"PER: {c.Stats.Perception}";
            _clickUi.Add(_branch);
            _shouldShow = true;
        }

        private void Hide()
        {
            _shouldShow = false;
            _clickUi.Remove(_branch); 
        }
        
        public void Draw(Transform2 parentTransform)
        {
            if (_shouldShow)
                _visuals.ForEach(x => x.Draw(parentTransform));
        }
    }
}
