
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class ActionSystem : MonoBehaviour
    {
        private GameManager m_gameManager;

        private bool m_gameState;

        void Start()
        {
            m_gameManager = FindObjectOfType<GameManager>();

            m_gameState = false;
        }

        void FixedUpdate()
        {

        }

        public void SetGameState(bool state)
        {
            m_gameState = state;

            if(m_gameState)
            {
                m_gameManager.LoadAP();
            }
            else
            {

            }
        }

        public bool GetGameState() { return m_gameState; }


        public void Win()
        {
            m_gameManager.Win();
        }
    }
}
