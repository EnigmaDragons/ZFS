using MonoDragons.Core.Engine;
using MonoDragons.Core.Physics;

namespace MonoDragons.Core.Scenes
{
    public abstract class SimpleScene : GameObjContainer, IScene
    {
        public abstract void Init();
        public virtual void Dispose() {  }

        public void Draw() => Draw(Transform2.Zero);
    }
}
