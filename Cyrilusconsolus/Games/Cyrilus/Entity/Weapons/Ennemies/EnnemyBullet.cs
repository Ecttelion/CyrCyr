using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Ships;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies
{
    public abstract class EnnemyBullet : EntityWithSprite
    {
        #region Properties of the ship

        public float Speed { get; set; } = 45;
        public float DmgMultiplayer { get; set; } = 1f;
        public Ship Shooter { get; }

        #endregion

        #region Var


        #endregion

        #region Constructor & funcs

        public EnnemyBullet(CyrilusGamus game, Ship shooter) : base(game)
        {
            Shooter = shooter;
        }


        public override void Update(float elapsedTime, KeyboardInput input)
        {
            if (Sprite.Position.Y < Game.ScreenHeight)
                Sprite.Position -= new Vector(0, -1f) * Speed * elapsedTime;
            else
                Remove = true;
        }

        public override void DrawLayer1(float elapsedTime, KeyboardInput input)
        {
            Game.DrawSprite(Sprite);
            base.DrawLayer1(elapsedTime, input);
        }

        #endregion



    }
}
