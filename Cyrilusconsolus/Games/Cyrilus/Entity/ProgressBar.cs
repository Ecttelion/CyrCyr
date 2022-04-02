using ConsoleGameEngine.Core.GameObjects;
using ConsoleGameEngine.Core.Input;
using ConsoleGameEngine.Core.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyrilusconsolus.Games.Cyrilus
{
    public class ProgressBar
    {
        #region Properties of the ship

        public int CharCount { get; set; } = 50;
        public float Max { get; set; } = 50;
        public float Value { get; set; } = 0;

        public string CharacterA { get; set; } = "0";
        public string CharacterB { get; set; } = " ";
        public string Text { get; set; } = "";

        #endregion


        #region Constructor & funcs

        public void Update(float elapsedTime, KeyboardInput input)
        {
            Text = "";
            var percent = Value * CharCount / Max ;
            for (int i = 0; i < CharCount; i++)
            {
                if (i > percent)
                    Text += CharacterB;
                else
                    Text += CharacterA;
            }
        }

        #endregion



    }
}
