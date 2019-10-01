using UnityEngine;

namespace Jelly
{
    public static class Formulas
    {
        public static int CalculateCountBlocks(int lvl)
        {
            int min = 3;

            return min + (int)(lvl / 10) * 2;
        }
    }
}