using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Friends;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Ships
{
    public class PlayerShip : Ship
    {
        #region Properties of the ship

        public float BoostQty { get; set; } = 5f;
        public float BoostUseRate { get; set; } = 1f;
        public float BoostRegenRate { get; set; } = 0.5f;

        public float SpeedBoostMultiplier { get; set; } = 3f;

        public float AttackCoolDown { get; set; } = 0.8f;
        public float AttackCoolDown2 { get; set; } = 1.6f;


        public ProgressBar CD2 { get; set; } = new();
        public ProgressBar BoostBar { get; set; } = new();
        public ProgressBar ShieldBar { get; set; } = new();
        public ProgressBar VitalityBar { get; set; } = new();


        #endregion

        #region Var

        public float CurrentAttackCoolDown { get; set; } = 0f;
        public float CurrentAttackCoolDown2 { get; set; } = 0f;
        public float CurrentBoost { get; set; } = float.PositiveInfinity;

        #endregion

        #region Constructor & funcs

        public PlayerShip(CyrilusGamus game) : base(game)
        {
            IsEnnemy = false;
            Gfx = "                :=  =.                \n            -=  -=  +. :=.            \n           ===  -=  +: :++.           \n          -+==  *+.:+= :+=+           \n  +#.    .**++ -*++*+*.:++*=     =#:  \n -+#.    *+=+#*++####++#+=+*-    =#=. \n:++*:   :+=++#*++*-+*+*#*++==    =*=+ \n===#*:  .+===#*+#=.=***#+==+-  .=#+=+.\n-==*#*===+++++++*=:++#+++++*+==+##+==:\n -=-===*#=**+**+********++#=*#+===-=. \n :==-=-*#=**++*++%#%#+**++#+*#=====+  \n    :::-=+*##*####***##**##*+--::.    \n         .:=-  =+++++=. :=-.          \n";
            Sprite = new Sprite(Gfx, ConsoleColor.Yellow);
            Sprite.Position = game.ScreenRect.Center;
            CD2.CharCount = 20;
            BoostBar.CharCount = 20;
            ShieldBar.CharCount = 20;
            VitalityBar.CharCount = 20;
            //Attack = 3;
            Speed = 30;
            ShieldQty = 7.5f;
        }

        public override void Update(float elapsedTime, KeyboardInput input)
        {
            if (CurrentBoost == float.PositiveInfinity)
                CurrentBoost = BoostQty;

            base.Update(elapsedTime, input);

            //calcul boost et vitesse de déplacement
            var dynamicSpeed = Speed;
            if (!collisions.Any(e => e is Vulkain))
            {
                if (input.IsKeyHeld(KeyCode.LShift) && CurrentBoost > 0)
                {
                    dynamicSpeed *= SpeedBoostMultiplier;
                    CurrentBoost -= BoostUseRate * elapsedTime;
                }
                else
                {
                    CurrentBoost += BoostRegenRate * elapsedTime;
                    if (CurrentBoost > BoostQty)
                    {
                        CurrentBoost = BoostQty;
                    }
                }

                //gestion du clavier pour déplacement
                if (input.IsKeyHeld(KeyCode.Z))
                {
                    if (Sprite.Position.Y > 0)
                        Sprite.Position += new Vector(0, -0.33f) * dynamicSpeed * elapsedTime;
                }
                if (input.IsKeyHeld(KeyCode.S))
                {
                    if (Sprite.Position.Y + Sprite.Height < Game.ScreenHeight)
                        Sprite.Position += new Vector(0, 0.33f) * dynamicSpeed * elapsedTime;
                }
                if (input.IsKeyHeld(KeyCode.Q))
                {
                    if (Sprite.Position.X > 0)
                        Sprite.Position += new Vector(-1, 0) * dynamicSpeed * elapsedTime;
                }
                if (input.IsKeyHeld(KeyCode.D))
                {
                    if (Sprite.Position.X + Sprite.Width < Game.ScreenWidth)
                        Sprite.Position += new Vector(1, 0) * dynamicSpeed * elapsedTime;
                }
            }

            //cooldownattak
            if (CurrentAttackCoolDown <= 0)
            {
                Game.Entities.Add(new BasicBullet(Game, this));
                CurrentAttackCoolDown = AttackCoolDown;
            }
            else
            {
                CurrentAttackCoolDown -= elapsedTime;
            }
            if (CurrentAttackCoolDown2 <= 0)
            {
                if (input.IsKeyHeld(KeyCode.Space))
                {
                    var ph = new Photon(Game, this);

                    var CenterOfShooterPos = Sprite.Bounds.Center;
                    var mouseOnConsolePos = MouseInput.GetMousePosition();
                    ph.Dir = new Vector(mouseOnConsolePos.X - CenterOfShooterPos.X, mouseOnConsolePos.Y - CenterOfShooterPos.Y).Normalized;

                    //ph.Speed = _random.Next(50, 1000);
                    Game.Entities.Add(ph);
                    CurrentAttackCoolDown2 = AttackCoolDown2;
                }
            }
            else
            {
                CurrentAttackCoolDown2 -= elapsedTime;
            }

            //gestions des bars 
            CD2.Max = AttackCoolDown2;
            CD2.Value = AttackCoolDown2 - CurrentAttackCoolDown2;
            CD2.Update(elapsedTime, input);

            BoostBar.Max = BoostQty;
            BoostBar.Value = CurrentBoost;
            BoostBar.Update(elapsedTime, input);

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
            //Game.DrawString(new Vector(0, 0), $"Boost      : {BoostBar.Text} ({Math.Round(BoostBar.Value, 1)}/{BoostBar.Max})", ConsoleColor.Red);
            //Game.DrawString(new Vector(0, 1), $"Shield     : {ShieldBar.Text} ({Math.Round(ShieldBar.Value, 1)}/{ShieldBar.Max})", ConsoleColor.Blue);
            //Game.DrawString(new Vector(0, 2), $"Vita       : {VitalityBar.Text} ({Math.Round(VitalityBar.Value, 1)}/{VitalityBar.Max})", ConsoleColor.Green);
            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y + entityBounds.Size.Y + 2), $"{CD2.Text} ({Math.Round(CD2.Value, 1)}/{CD2.Max})", ConsoleColor.Cyan, centered: true);
            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y + entityBounds.Size.Y + 3), $"{BoostBar.Text} ({Math.Round(BoostBar.Value, 1)}/{BoostBar.Max})", ConsoleColor.Red, centered: true);
            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y + entityBounds.Size.Y + 4), $"{ShieldBar.Text} ({Math.Round(ShieldBar.Value, 1)}/{ShieldBar.Max})", ConsoleColor.Blue, centered: true);
            Game.DrawString(new Vector(entityCenter.X, entityBounds.Position.Y + entityBounds.Size.Y + 5), $"{VitalityBar.Text} ({Math.Round(VitalityBar.Value, 1)}/{VitalityBar.Max})", ConsoleColor.Green, centered: true);
            base.DrawLayerUI(elapsedTime, input);
        }

        #endregion
    }
}
