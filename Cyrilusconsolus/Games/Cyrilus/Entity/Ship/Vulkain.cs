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
    public class Vulkain : Ship
    {
        #region Properties of the ship

        public string Gfx { get; } = "                  :                  \n                 *#+                 \n                #%*@+                \n              .+@%*@@=               \n            .:#%%%*@%%+:             \n           :%%%%%%%%%%@%#            \n         .=#%@%#%%%%##%@%*-          \n      :+##@%%@%%#####%@@%%%#*=.      \n    .#%%%%@%%@%%%###%%%@%%@%%%%+     \n   .%%%%%%%#%@#%@%*%@%%@##@%%%%%*    \n   +@%%#%@%*%@%%@@@@%%%@##@%#%%%@:   \n  :%#@%+::+=#%@@@@%@@@@@+==:-*@%##   \n .###-       +. .--:. =-      .+%#*  \n #%+                            .*%= \n-#.                               =% \n-                                  .:\n";
        public float AttackCoolDown { get; set; } = 1.2f;


        public ProgressBar ShieldBar { get; set; } = new();
        public ProgressBar VitalityBar { get; set; } = new();

        public bool Dead => CurrentVitality < 0;

        #endregion

        #region Var

        private float _currentAttackCoolDown = 0f;


        #endregion

        #region Constructor & funcs

        public Vulkain(CyrilusGamus game, int x) : base(game)
        {
            //Gfx = gfx;
            Sprite = new Sprite(Gfx, ConsoleColor.Gray);
            Sprite.Position =  new Vector(x, 0);

            ShieldBar.CharCount = 10;
            VitalityBar.CharCount = 10;
        }

        public override void Update(float elapsedTime, KeyboardInput input)
        {

            //cooldownattak
            if (_currentAttackCoolDown <= 0)
            {
                Game.Entities.Add(new VulkainBullet(Game, this));
                _currentAttackCoolDown = AttackCoolDown;
            }
            else
            {
                _currentAttackCoolDown -= elapsedTime;
            }

            //gestions des bars 

            ShieldBar.Max = ShieldQty;
            ShieldBar.Value = CurrentShield;
            ShieldBar.Update(elapsedTime, input);

            VitalityBar.Max = Vitality;
            VitalityBar.Value = CurrentVitality;
            VitalityBar.Update(elapsedTime, input);
        }

        public override void DrawLayerUI(float elapsedTime, KeyboardInput input)
        {
            var entityBounds = Sprite.Bounds;
            var entityCenter = entityBounds.Center;

            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y - 1), $"{ShieldBar.Text}", ConsoleColor.Blue, centered: true);
            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y - 2), $"{VitalityBar.Text}", ConsoleColor.Green, centered: true);
            base.DrawLayerUI(elapsedTime, input);
        }

        #endregion



    }
}
