using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelly
{
    public class EnemyGenerator : MonoBehaviour
    {
        private ActionSystem m_actionSystem;
        private GameManager m_gameManager;

        [SerializeField] private List<Enemy> m_enemyPrefabs;

        [SerializeField] private int m_minDistToCreate;
        [SerializeField] private int m_maxDistToCreate;

        void Start()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();
            m_gameManager = FindObjectOfType<GameManager>();

            UpdateSetting();
        }

        public void UpdateSetting()
        {

        }

        void FixedUpdate()
        {

        }
    }
}
