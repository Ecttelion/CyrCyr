using ConsoleGameEngine.Core;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using Cyrilusconsolus.Games.Cyrilus.Entity;
using Cyrilusconsolus.Games.Cyrilus.Entity.Ships;

namespace Cyrilusconsolus.Games.Cyrilus
{
    public class CyrilusGamus : ConsoleGameEngineBase
    {

        public const int ConsoleWidth = 383;
        public const int ConsoleHeight = 131;
        public const int ConsoleTextSize = 10;

        protected override string Name => "CyrilusGamus";

        public Random Random = new Random();


        public PlayerShip Ship { get; set; }
        public List<Entity.Entity> Entities { get; set; } = new();

        //private Sprite _boost;

        public CyrilusGamus()
        {
            InitConsole(ConsoleWidth, ConsoleHeight, ConsoleTextSize);

            Ship = new(this);
        }
        protected override bool Create()
        {
            Entities.Add(Ship);
            //for (int i = 0; i < 7; i++)
            //{
            //    Entities.Add(new Vulkain(this, i * 55));
            //}
            Entities.Add(new Romulien(this, 20));
            return true;
        }


        protected override bool Update(float elapsedTime, KeyboardInput input)
        {
            //if (Entities.Count == 1)
            //{
            //    Entities.Add(new Romulien(this, 20));
            //}

            if (input.IsKeyHeld(KeyCode.Esc))
            {
                return false;
            }

            //nettoyage de la console
            Fill(ScreenRect, ' ', bgColor: ConsoleColor.Black);


            //UPDATE GAME

            var entities = Entities.ToArray();
            var entitiesWS = Entities.Where(e => e is EntityWithSprite).Select(e => (EntityWithSprite)e).ToArray();
            foreach (var item in entities)
            {
                if (item is EntityWithSprite _item)
                {
                    _item.collisions.Clear();
                    foreach (var item2 in entitiesWS)
                    {
                        if (_item == item2)
                            continue;

                        if (Rect.Intersect(_item.Sprite.Bounds, item2.Sprite.Bounds))
                        {
                            _item.collisions.Add(item2);
                        }
                    }
                }
                item.Update(elapsedTime, input);


                if (item.Remove)
                    Entities.Remove(item);
            }

            //DRAW GAME

            foreach (var item in Entities)
            {
                item.DrawLayer1(elapsedTime, input);
            }

            //DRAW UI

            foreach (var item in Entities)
            {
                item.DrawLayerUI(elapsedTime, input);
            }

            return true;
        }
    }
}
