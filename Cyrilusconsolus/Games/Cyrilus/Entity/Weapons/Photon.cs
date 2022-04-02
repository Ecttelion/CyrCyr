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
    public class Photon : EntityWithSprite
    {
        #region Properties of the ship

        public string Gfx { get; } = "           :          \n          :=.         \n         .:.=         \n         :  -:        \n.:==:.::..   :::::==-.\n   .--.        .-=:   \n      :-      -=      \n      :.  ::   =      \n     .-:--..:-.:-     \n     ==:      .:=:    \n    ..           :    \n";
        public float Speed { get; set; } = 500;

        public EntityWithSprite Shooter { get; }

        public int ColorIndex = 0;
        public float Counter = 0;


        #endregion

        #region Var


        #endregion

        #region Constructor & funcs

        public Photon(CyrilusGamus game, EntityWithSprite shooter) : base(game)
        {
            Shooter = shooter;
            Sprite = new Sprite(Gfx, ConsoleColor.Cyan);
            Sprite.Position = shooter.Sprite.Bounds.Center - new Vector(Sprite.Width / 2, shooter.Sprite.Bounds.Size.Y / 2);
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
