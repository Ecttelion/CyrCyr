using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity.Ships;

namespace Cyrilusconsolus.Games.Cyrilus.Entity.Weapons.Ennemies
{
    public class VulkainBullet : EnnemyBullet
    {
        #region Properties of the ship


        #endregion

        #region Var

        #endregion

        #region Constructor & funcs

        public VulkainBullet(CyrilusGamus game, Ship shooter) : base(game, shooter)
        {
            Gfx = "       .....     \n    .:: ...::-.  \n   :=---:::-=+==.\n :==-:.       ...\n:.               \n";
            Sprite = new Sprite(Gfx, ConsoleColor.Red);
            Sprite.Position = shooter.Sprite.Bounds.Center - new Vector(Sprite.Width / 2, -shooter.Sprite.Bounds.Size.Y / 2);
        }

        #endregion

    }
}
