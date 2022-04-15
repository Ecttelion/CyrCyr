using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Ships
{
    public class Romulien : Ship
    {
        #region Properties of the ship

        public float AttackCoolDown { get; set; } = 1.8f;
        public float Attack2CoolDown { get; set; } = 3f;


        public ProgressBar ShieldBar { get; set; } = new();
        public ProgressBar VitalityBar { get; set; } = new();


        #endregion

        #region Var

        private float _currentAttackCoolDown = 0f;
        private float _currentAttack2CoolDown = 0f;


        #endregion

        #region Constructor & funcs

        public Romulien(CyrilusGamus game, int x) : base(game)
        {
            Gfx = "              ......              \n         :=*#####*####*=:         \n      -*%%%%%#**%%**#%%%%%*-      \n    +%%#*%@%%##%%%%##%%@%*#%%+    \n  :%@%**%%*+=-*%%%%*-=+*%%**%@%:  \n :@@%%#@%**:=#%#%%#@#=:**%@#%%@@: \n %@%%%@%%%#=#%@@##@@%#=#%%%@%%%@% \n-@%%#%%%%%%*+#%@**@%#+*%%%%%%#%%@-\n+@%##@%*##*+#%@=  =@%#+*##*%@##%@+\n+%%%@@#++++#@%=    =%@%++++#@@%%%+\n=@@@%%#+=+%+..:    :..+%+=+#%%@@@=\n.%@%%##**%#.          .#%**##%%@%.\n .%%%%##%#%%-        -%%#%##%%%%: \n  -@@%#%%  :=-      -=:  %%#%@@-  \n   -@%%%@:              :@%%%@-   \n    #%%@@%-            :%@@%%#.   \n     -#@@@@+.        .+%@@@#-     \n       :+%%+**-    -**+%%+:       \n          .-: ..   . :-:          \n";
            Sprite = new Sprite(Gfx, ConsoleColor.Gray);
            Sprite.Position = new Vector(x, 20);
            ShieldBar.CharCount = 10;
            VitalityBar.CharCount = 20;
            Speed = 35;
            ShieldQty = 10;
            VitalityQty = 50;
            AttackQty = 5;
        }

        public override void Update(float elapsedTime, KeyboardInput input)
        {
            base.Update(elapsedTime, input);

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

            //cooldownattak
            if (_currentAttack2CoolDown <= 0)
            {
                var ph = new PhotonGuided(Game, this);
                var shooterPos = Sprite.Bounds.Center;
                var targetPos = Game.Ship.Sprite.Bounds.Center;
                ph.Dir = new Vector(targetPos.X - shooterPos.X, targetPos.Y - shooterPos.Y).Normalized;

                Game.Entities.Add(ph);
                _currentAttack2CoolDown = Attack2CoolDown;
            }
            else
            {
                _currentAttack2CoolDown -= elapsedTime;
            }

            //gestions des bars 

            ShieldBar.Max = ShieldQty;
            ShieldBar.Value = CurrentShield;
            ShieldBar.Update(elapsedTime, input);

            VitalityBar.Max = VitalityQty;
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
