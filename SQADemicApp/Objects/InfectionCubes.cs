using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp
{
    public class InfectionCubes
    {
        private Dictionary<COLOR, int> cubesSet;

        public InfectionCubes() : this(0)
        {
        }


        public InfectionCubes(int startingValue)
        {
            cubesSet = new Dictionary<COLOR, int>();
            var values = Enum.GetValues(typeof (COLOR)).Cast<COLOR>();
            foreach (var color in values)
            {
                cubesSet.Add(color, startingValue);
            }
        }


        public void DecrementCubeCountWithGameOver(COLOR color)
        {
            cubesSet[color] --;
            if (cubesSet[color] <= 0)
            {
                throw new InvalidOperationException("Game Over");
            }
        }

        public void DecrementCubeCount(COLOR color)
        {
            cubesSet[color]--;
        }


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