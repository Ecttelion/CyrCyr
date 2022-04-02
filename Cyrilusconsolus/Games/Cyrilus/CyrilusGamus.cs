using System;
using ConsoleGameEngine.Core;
using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity;

namespace Cyrilusconsolus.Games.Cyrilus
{
    public class CyrilusGamus : ConsoleGameEngineBase
    {

        protected override string Name => "CyrilusGamus";

        public Random Random = new Random();


        public PlayerShip Ship { get; set; }
        public List<Entity.Entity> Entities { get; set; } = new();

        //private Sprite _boost;

        public CyrilusGamus()
        {
            InitConsole(383, 131, 11);

            Ship = new(this);
        }
        protected override bool Create()
        {
            for (int i = 0; i < 7; i++)
            {
                Entities.Add(new Vulkain(this, i * 55));
            }
            return true;
        }


        protected override bool Update(float elapsedTime, KeyboardInput input)
        {
            if (input.IsKeyHeld(KeyCode.Esc))
            {
                return false;
            }

            //nettoyage de la console
            Fill(ScreenRect, ' ', bgColor: ConsoleColor.Black);


            //UPDATE GAME

            Ship.Update(elapsedTime, input);
            var tmpBullet = Entities.ToArray();
            foreach (var item in tmpBullet)
            {
                item.Update(elapsedTime, input);
                if (item.Remove)
                    Entities.Remove(item);
            }

            //DRAW GAME

            Ship.DrawLayer1(elapsedTime, input);
            foreach (var item in Entities)
            {
                item.DrawLayer1(elapsedTime, input);
            }

            //DRAW UI

            Ship.DrawLayerUI(elapsedTime, input);
            foreach (var item in Entities)
            {
                item.DrawLayerUI(elapsedTime, input);
            }

            return true;
        }
    }
}
