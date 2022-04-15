using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;

namespace Cyrilusconsolus.Games.Cyrilus.Entity
{
    public abstract class EntityWithSprite : Entity
    {
        public string Gfx { get; set; } = "*";
        public Sprite Sprite { get; set; }

        public List<EntityWithSprite> collisions = new();

        public EntityWithSprite(CyrilusGamus game) : base(game)
        {
        }

        public override void DrawLayer1(float elapsedTime, KeyboardInput input)
        {
            Game.DrawSprite(Sprite);
            base.DrawLayer1(elapsedTime, input);
        }

    }
}
