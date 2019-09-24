using UnityEngine;

namespace Jelly
{
    public static class Formulas
    {
        private static int m_maxLevel = 100; //after this level speed and count of enemies will be the same

        public static float GetEnemyTimeToCreate(int lvl)
        {
            float min = 2.0f;
            float max = 1.0f;

            return min + (lvl / m_maxLevel) * (max - min);
        }

        public static float GetEnemySpeed(int lvl)
        {
            float min = 3.0f;
            float max = 5.0f;

            return min + (lvl / m_maxLevel) * (max - min);
        }

        public static int GetNeedCountCoin(int lvl)
        {
            return 10 + lvl / 10;
        }
    }
}