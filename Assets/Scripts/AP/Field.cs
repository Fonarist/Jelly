using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Vector2 m_startPos; //left top corner of game field

        [SerializeField] private int m_width;
        [SerializeField] private int m_height;

        [SerializeField] private float m_offset;

        private List<Enemy> m_enemies;
        private GameObject m_coin;

        protected void Awake()
        {
            m_enemies = new List<Enemy>();

            m_startPos += new Vector2(transform.position.x, transform.position.y);
        }

        public int GetWidth() { return m_width; }

        public int GetHeight() { return m_height; }

        public float GetOffset() { return m_offset; }

        public Vector2 GetStartPos() { return m_startPos; }

        public Vector2 GetWorldPos(int w, int h)
        {
            return new Vector2(m_startPos.x + m_offset * w, m_startPos.y - m_offset * h);
        }

        public void AddEnemy(Enemy enemy)
        {
            m_enemies.Add(enemy);
        }

        public void DeleteEnemy(Enemy enemy)
        {
            m_enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        public void DeleteEnemies()
        {
            for (int i = 0; i < m_enemies.Count; ++i)
            {
                Destroy(m_enemies[i].gameObject);
            }

            m_enemies.Clear();
        }

        public IEnumerator<Enemy> GetEnemies(){ return m_enemies.GetEnumerator(); }

        public int GetEnemiesCount() { return m_enemies.Count; }

        public void AddCoin(GameObject coin)
        {
            m_coin = coin;
        }

        public void DeleteCoin()
        {
            if (m_coin)
            {
                Destroy(m_coin);
                m_coin = null;
            }
        }

        public bool HasCoin() { return m_coin != null; }
    }
}
