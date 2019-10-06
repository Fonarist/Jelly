using UnityEngine;

namespace Jelly
{
    public static class Formulas
    {
        public static int CalculateCountBlocks(int lvl)
        {
            int min = 3;

            return min + (int)(lvl / 10);
        }

        public static int CalculatePlayerSpeed(int lvl)
        {
            int min = 12;
            int max = 25;

            int speed = min + (int)(lvl / 2);
            if(speed > max)
            {
                speed = max;
            }

            return speed;
        }
    }
}