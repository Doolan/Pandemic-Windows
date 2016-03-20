using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp
{
    public abstract class InfectionCubes
    {
        protected Dictionary<COLOR, int> cubesSet;

        protected InfectionCubes() : this(0)
        {
        }

        protected InfectionCubes(int startingValue)
        {
            cubesSet = new Dictionary<COLOR, int>();
            var values = Enum.GetValues(typeof (COLOR)).Cast<COLOR>();
            foreach (var color in values)
            {
                cubesSet.Add(color, startingValue);
            }
        }

        public void IncrementCubes(COLOR color)
        {
            cubesSet[color]++;
        }

        public abstract void DecrementCubeCount(COLOR color);

        public int GetCubeCount(COLOR color)
        {
            return cubesSet[color];
        }

        public void AddCubes(COLOR color, int value)
        {
            cubesSet[color] += value;
        }

        public void SetCubeCount(COLOR color, int value)
        {
            cubesSet[color] = value;
        }
    }
}