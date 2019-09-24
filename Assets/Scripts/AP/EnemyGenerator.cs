using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class EnemyGenerator : MonoBehaviour
    {
        private Field m_field;
        private ActionSystem m_actionSystem;
        private GameManager m_gameManager;

        [System.Serializable]
        private class Path
        {
            public float m_startX;
            public float m_startY;
            public float m_endX;
            public float m_endY;
        }

        [SerializeField] private Enemy m_enemyPrefab;

        private float m_timeToCreate;
        private float m_timerToCreate;

        private float m_enemySpeed;

        [SerializeField] List<Path> m_paths;

        void Start()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_field = FindObjectOfType<Field>();
            m_gameManager = FindObjectOfType<GameManager>();

            UpdateSetting();
        }

        public void UpdateSetting()
        {
            m_timeToCreate = Formulas.GetEnemyTimeToCreate(m_gameManager.GetLevel());
            m_timerToCreate = 0;

            m_enemySpeed = Formulas.GetEnemySpeed(m_gameManager.GetLevel());
        }

        void FixedUpdate()
        {

            if(m_timerToCreate > 0)
            {
                m_timerToCreate -= Time.deltaTime;
            }
            else
            {
                Enemy obj = Instantiate<Enemy>(m_enemyPrefab, m_field.transform);

                int rand = 0;
                do
                {
                    rand = Random.Range(0, m_paths.Count);
                } while (!m_actionSystem.GetGameState() && (m_paths[rand].m_startX == 0 || m_paths[rand].m_startY == 0));


                obj.transform.position = new Vector3(m_paths[rand].m_startX, m_paths[rand].m_startY, 0);
                obj.MoveTo(new Vector3(m_paths[rand].m_endX, m_paths[rand].m_endY, 0));

                m_field.AddEnemy(obj);

                m_timerToCreate = m_timeToCreate;
            }
        }
    }
}
