using System;
using MonoDragons.Core.Physics;

namespace MonoDragons.Core.Scenes
{
    public sealed class CurrentScene : IScene
    {
        public IScene Value { get; private set; }

        public void Set(IScene scene) => Value = scene;
        public void Init() => Value = new PlaceholderScene();
        public void Update(TimeSpan delta) => Value.Update(delta);
        public void Draw(Transform2 parentTransform) => Value.Draw(parentTransform);
        public void Dispose() => Value.Dispose();
    }
}
