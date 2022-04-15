using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Ships;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Friends
{
    public abstract class FriendBullet : EntityWithSprite
    {
        #region Properties of the ship

        public float Speed { get; set; } = 50;
        public float DmgMultiplayer { get; set; } = 1f;
        public Ship Shooter { get; }


        #endregion

        #region Var


        #endregion

        #region Constructor & funcs

        public FriendBullet(CyrilusGamus game, Ship shooter) : base(game)
        {
            Shooter = shooter;
        }


        public override void Update(float elapsedTime, KeyboardInput input)
        {
            if (Sprite.Position.Y > 0)
                Sprite.Position += new Vector(0, -1) * Speed * elapsedTime;
            else
                Remove = true;

            var resultShip = collisions.Where(e => e is EnnemyBullet).Select(e => (EnnemyBullet)e).ToArray();
            foreach (var item in resultShip)
            {
                Remove = true;
                item.Remove = true;



            }
        }

        public override void DrawLayer1(float elapsedTime, KeyboardInput input)
        {
            Game.DrawSprite(Sprite);
            base.DrawLayer1(elapsedTime, input);
        }

        #endregion



    }
}
