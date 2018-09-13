
using MonoDragons.ZFS.Characters;

namespace MonoDragons.ZFS.GUI
{
    public class AttackAnimationsFinished
    {
        public Character Attacker { get; }
        public Character Target { get; }

        public AttackAnimationsFinished(Character attacker, Character target)
        {
            Attacker = attacker;
            Target = target;
        }
    }
}
