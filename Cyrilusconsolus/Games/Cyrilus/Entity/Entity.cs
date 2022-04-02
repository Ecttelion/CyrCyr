using ConsoleGameEngine.Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyrilusconsolus.Games.Cyrilus.Entity
{
    public abstract class Entity
    {

        public CyrilusGamus Game { get; set; }
        public bool Remove { get; set; } = false;

        public Entity(CyrilusGamus game)
        {
            Game = game;
        }

        public abstract void Update(float elapsedTime, KeyboardInput input);

        public virtual void DrawLayer1(float elapsedTime, KeyboardInput input)
        {

        }
        public virtual void DrawLayerUI(float elapsedTime, KeyboardInput input)
        {

        }

    }
}
