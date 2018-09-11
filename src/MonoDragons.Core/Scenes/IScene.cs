using System;
using MonoDragons.Core.Engine;

namespace MonoDragons.Core.Scenes
{
    public interface IScene: IDisposable, IInitializable, IVisual, IAutomaton
    {
    }
}
