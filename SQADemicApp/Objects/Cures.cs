using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp.Objects
{
    public class Cures
    {
        public enum CURESTATE
        {
            NotCured,
            Cured,
            Sunset
        }

        private readonly Dictionary<COLOR, CURESTATE> cureSet;

        public Cures() : this(CURESTATE.NotCured)
        {
        }

        public Cures(CURESTATE state)
        {
            cureSet = new Dictionary<COLOR, CURESTATE>();
            var values = Enum.GetValues(typeof (COLOR)).Cast<COLOR>();
            foreach (var color in values)
            {
                cureSet.Add(color, state);
            }
        }

        public CURESTATE GetCureStatus(COLOR color)
        {
            return cureSet[color];
        }

        public void SetCureStatus(COLOR color, CURESTATE curestate)
        {
            cureSet[color] = curestate;
        }
    }
}