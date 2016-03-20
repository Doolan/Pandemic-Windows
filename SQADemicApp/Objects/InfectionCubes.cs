using System;

namespace SQADemicApp
{
    public class InfectionCubes
    {

        public void DecrementCubeCount(COLOR color)
        {
            //temporary switch statement -- will change when underlying storage updated
            switch (color)
            {
                case COLOR.red:
                    redCubes --;
                    if(redCubes<= 0)
                        throw new InvalidOperationException("Game Over");
                    break;
                case COLOR.black:
                    blackCubes--;
                    if (blackCubes <= 0)
                        throw new InvalidOperationException("Game Over");
                    break;
                case COLOR.blue:
                    blueCubes--;
                    if (blueCubes <= 0)
                        throw new InvalidOperationException("Game Over");
                    break;
                case COLOR.yellow:
                    yellowCubes--;
                    if (yellowCubes <= 0)
                        throw new InvalidOperationException("Game Over");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public int GetCubeCount(COLOR color)
        {
            //temporary switch statement -- will change when underlying storage updated
            switch (color)
            {
                case COLOR.red:
                    return redCubes;
                    
                case COLOR.black:
                    return blackCubes;
                case COLOR.blue:
                    return blueCubes;
                case COLOR.yellow:
                    return yellowCubes;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public void AddCubes(COLOR color, int value)
        {
            //temporary switch statement -- will change when underlying storage updated
            switch (color)
            {
                case COLOR.red:
                    redCubes += value;
                    break;
                case COLOR.black:
                    blackCubes += value;
                    break;
                case COLOR.blue:
                    blueCubes += value;
                    break;
                case COLOR.yellow:
                    yellowCubes += value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public int redCubes { get; set; }
        public int blackCubes { get; set; }
        public int blueCubes { get; set; }
        public int yellowCubes { get; set; }
    }
}