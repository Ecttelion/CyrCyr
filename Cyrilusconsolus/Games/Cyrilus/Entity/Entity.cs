using ConsoleGameEngine.Core.Input;

namespace Cyrilusconsolus.Games.Cyrilus.Entity
{
    public abstract class Entity
    {

        public CyrilusGamus Game { get; set; }
        public bool Remove { get; set; } = false;

        public Entity(CyrilusGamus game)
        {
            Game = game;
        }

        public abstract void Update(float elapsedTime, KeyboardInput input);

        public virtual void DrawLayer1(float elapsedTime, KeyboardInput input)
        {

        }
        public virtual void DrawLayerUI(float elapsedTime, KeyboardInput input)
        {

        }

    }
}
