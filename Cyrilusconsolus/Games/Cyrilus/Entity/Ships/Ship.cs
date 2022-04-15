using ConsoleGameEngine.Core.Input;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies;
using Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Friends;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Ships
{
    public class Ship : EntityWithSprite
    {
        #region Properties of the ship

        public bool IsEnnemy { get; set; } = true;

        public float VitalityQty { get; set; } = 5f;
        public float AttackQty { get; set; } = 2.5f;
        public float ShieldQty { get; set; } = 5f;

        public float ShieldRegenRate { get; set; } = 1f;

        public float Speed { get; set; } = 25;

        #endregion

        #region Var


        public float CurrentShield { get; set; } = float.PositiveInfinity;
        public float CurrentVitality { get; set; } = float.PositiveInfinity;


        #endregion

        #region Constructor & funcs

        public Ship(CyrilusGamus game) : base(game)
        {
        }

        public override void Update(float elapsedTime, KeyboardInput input)
        {
            //gestion Vita / Shield
            if (CurrentShield == float.PositiveInfinity)
                CurrentShield = ShieldQty;

            if (CurrentShield < ShieldQty)
                CurrentShield += ShieldRegenRate * elapsedTime;

            if (CurrentVitality == float.PositiveInfinity)
                CurrentVitality = VitalityQty;

            if (CurrentVitality <= 0)
                Remove = true;

            var resultShip = collisions.Where(e => e is Ship).Select(e => (Ship)e).ToArray();
            foreach (var item in resultShip)
            {
                var totalItem = item.CurrentShield + item.CurrentVitality;
                var total = CurrentShield + CurrentVitality;

                if (totalItem < total)
                {
                    item.CurrentVitality = 0;
                    item.Remove = true;
                    TakeDamage(totalItem);
                }
                else
                {
                    CurrentVitality = 0;
                    Remove = true;
                    item.TakeDamage(total);
                }
            }

            if (IsEnnemy)
            {
                var result = collisions.Where(e => e is FriendBullet).Select(e => (FriendBullet)e).ToArray();
                foreach (var item in result)
                {
                    item.Remove = true;
                    var dmg = item.DmgMultiplayer * item.Shooter.AttackQty;
                    TakeDamage(dmg);
                }
            }
            else
            {
                var result = collisions.Where(e => e is EnnemyBullet).Select(e => (EnnemyBullet)e).ToArray();
                foreach (var item in result)
                {
                    item.Remove = true;
                    var dmg = item.DmgMultiplayer * item.Shooter.AttackQty;
                    TakeDamage(dmg);
                }
            }


        }

        private void TakeDamage(float dmg)
        {
            if (CurrentShield > 0)
            {
                CurrentShield -= dmg;
                if (CurrentShield < 0)
                    CurrentVitality = CurrentVitality + CurrentShield;
            }
            else
            {
                CurrentVitality = CurrentVitality - dmg;
            }
        }

        #endregion

    }
}
