using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp
{
    public abstract class InfectionCubes
    {
        protected Dictionary<Color, int> CubesSet;

        protected InfectionCubes() : this(0)
        {
        }

        protected InfectionCubes(int startingValue)
        {
            CubesSet = new Dictionary<Color, int>();
            var values = Enum.GetValues(typeof (Color)).Cast<Color>();
            foreach (var color in values)
            {
                CubesSet.Add(color, startingValue);
            }
        }

        public void IncrementCubes(Color color)
        {
            CubesSet[color]++;
        }

        public abstract void DecrementCubeCount(Color color);

        public int GetCubeCount(Color color)
        {
            return CubesSet[color];
        }

        public void AddCubes(Color color, int value)
        {
            CubesSet[color] += value;
        }

        public void SetCubeCount(Color color, int value)
        {
            CubesSet[color] = value;
        }
    }
}