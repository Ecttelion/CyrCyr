using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyrilusconsolus.Games.Cyrilus.Entity
{
    public class Bullet : EntityWithSprite
    {
        #region Properties of the ship

        public string Gfx { get; } = "       .....     \n    .:: ...::-.  \n   :=---:::-=+==.\n :==-:.       ...\n:.               \n";
        public float Speed { get; } = 150;

        public EntityWithSprite Shooter { get; }

        #endregion

        #region Var


        #endregion

        #region Constructor & funcs

        public Bullet(CyrilusGamus game, EntityWithSprite shooter) : base(game)
        {
            Shooter = shooter;
            Sprite = new Sprite(Gfx, ConsoleColor.Magenta);
            Sprite.Position = shooter.Sprite.Bounds.Center - new Vector(Sprite.Width / 2, shooter.Sprite.Bounds.Size.Y / 2);
        }


        public override void Update(float elapsedTime, KeyboardInput input)
        {
            if (Sprite.Position.Y > 0)
                Sprite.Position += new Vector(0, -0.33f) * Speed * elapsedTime;
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
