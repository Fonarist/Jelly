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
            int min = 18;
            int max = 30;

            int speed = min + (int)(lvl / 2);
            if(speed > max)
            {
                speed = max;
            }

            return speed;
        }
    }
}