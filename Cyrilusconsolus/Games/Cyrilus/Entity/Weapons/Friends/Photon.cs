using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Ships;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Friends
{
    public class Photon : FriendBullet
    {

        #region Properties of the ship

        public int ColorIndex = 0;
        public float Counter = 0;
        public Vector Dir = Vector.Down;

        #endregion

        #region Var


        #endregion

        #region Constructor & funcs

        public Photon(CyrilusGamus game, Ship shooter) : base(game, shooter)
        {
            Gfx = "           :          \n          :=.         \n         .:.=         \n         :  -:        \n.:==:.::..   :::::==-.\n   .--.        .-=:   \n      :-      -=      \n      :.  ::   =      \n     .-:--..:-.:-     \n     ==:      .:=:    \n    ..           :    \n";
            Sprite = new Sprite(Gfx, ConsoleColor.Cyan);
            Sprite.Position = shooter.Sprite.Bounds.Center - new Vector(Sprite.Width / 2, shooter.Sprite.Bounds.Size.Y / 2);
            this.Speed = game.Random.Next(50, 500);
            this.DmgMultiplayer = game.Random.Next(10, 30) / 10f;
        }


        public override void Update(float elapsedTime, KeyboardInput input)
        {
            Counter -= elapsedTime;
            if (Counter < 0)
            {
                ColorIndex++;
                if (ColorIndex > 15)
                    ColorIndex = 0;
                Sprite.SetSpriteColor((ConsoleColor)ColorIndex);
                Counter = 0.1f;

            }

            if (Sprite.Position.Y < Game.ScreenHeight && Sprite.Position.Y > 0 && Sprite.Position.X > 0 && Sprite.Position.X + Sprite.Width < Game.ScreenWidth)
            {
                Sprite.Position += Dir * Speed * elapsedTime;//new Vector(mouseOnConsolePos.X, mouseOnConsolePos.Y); //
            }
            else
                Remove = true;

            var resultShip = collisions.Where(e => e is EnnemyBullet).Select(e => (EnnemyBullet)e).ToArray();
            foreach (var item in resultShip)
            {
                Remove = true;
                item.Remove = true;
            }

        }

        #endregion

    }
}
