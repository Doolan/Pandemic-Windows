using System;

namespace SQADemicApp.Objects
{
    public class Cures
    {
        public enum CURESTATE { NotCured, Cured, Sunset }
        public CURESTATE RedCure { get; set; }
        public CURESTATE BlueCure { get; set; }
        public CURESTATE BlackCure { get; set; }
        public CURESTATE YellowCure { get; set; }

        public Cures() :this(CURESTATE.NotCured)
        {
        }

        public Cures(CURESTATE state)
        {
            RedCure = state;
            BlueCure = state;
            BlackCure = state;
            YellowCure = state;
        }

        public CURESTATE getCureStatus(COLOR color)
        {
            switch (color)
            {
                case COLOR.red:
                    return RedCure;
                case COLOR.blue:
                    return BlueCure;
                case COLOR.yellow:
                    return YellowCure;
                case COLOR.black:
                    return BlackCure;
                default:
                    throw new ArgumentException("Not a vaild color");
            }
        }

        public void setCureStatus(COLOR color, CURESTATE curestate)
        {
            switch (color)
            {
                case COLOR.red:
                    RedCure = curestate;
                    break;
                case COLOR.blue:
                    BlueCure = curestate;
                    break;
                case COLOR.yellow:
                    YellowCure = curestate;
                    break;
                case COLOR.black:
                    BlackCure = curestate;
                    break;
                default:
                    throw new ArgumentException("Not a vaild color");
            }
        }


        


    }
}