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
    public class Ship : EntityWithSprite
    {
        #region Properties of the ship

        public float ShieldQty { get; set; } = 5f;
        public float ShieldRegenRate { get; set; } = 0.1f;

        public float Speed { get; set; } = 25f;

        public float Vitality { get; set; } = 5f;
        public float Attack { get; set; } = 5f;

        #endregion

        #region Var


        public float CurrentShield { get; set; } = 0f;
        public float CurrentVitality { get; set; } = float.PositiveInfinity;


        #endregion

        #region Constructor & funcs

        public Ship(CyrilusGamus game) : base(game)
        {
        }

        public override void Update(float elapsedTime, KeyboardInput input)
        {
            //gestion Vita / Shield

            if (CurrentShield < ShieldQty)
                CurrentShield += ShieldRegenRate * elapsedTime;

            if (CurrentVitality == float.PositiveInfinity)
                CurrentVitality = Vitality;
        }

        #endregion

    }
}
