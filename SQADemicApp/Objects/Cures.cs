using System;
using System.Collections.Generic;
using System.Linq;

namespace SQADemicApp.Objects
{
    public class Cures
    {
        public enum Curestate
        {
            NotCured,
            Cured,
            Sunset
        }

        private readonly Dictionary<Color, Curestate> _cureSet;

        public Cures() : this(Curestate.NotCured)
        {
        }

        public Cures(Curestate state)
        {
            _cureSet = new Dictionary<Color, Curestate>();
            var values = Enum.GetValues(typeof (Color)).Cast<Color>();
            foreach (var color in values)
            {
                _cureSet.Add(color, state);
            }
        }

        public Curestate GetCureStatus(Color color)
        {
            return _cureSet[color];
        }

        public void SetCureStatus(Color color, Curestate curestate)
        {
            _cureSet[color] = curestate;
        }
    }
}