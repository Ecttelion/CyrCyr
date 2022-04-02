using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyrilusconsolus.Games.Cyrilus.Entity
{
    public abstract class EntityWithSprite : Entity
    {

        public Sprite Sprite { get; set; }

        public EntityWithSprite(CyrilusGamus game) : base(game)
        {
            Game = game;
        }

        public override void DrawLayer1(float elapsedTime, KeyboardInput input)
        {
            Game.DrawSprite(Sprite);
            base.DrawLayer1(elapsedTime, input);
        }

    }
}
