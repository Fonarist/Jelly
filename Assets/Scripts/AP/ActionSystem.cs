
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class ActionSystem : MonoBehaviour
    {
        private GameManager m_gameManager;

        private bool m_gameState;
        private bool m_isWin;

        void Start()
        {
            m_gameManager = FindObjectOfType<GameManager>();

            m_gameState = false;
            m_isWin = false;
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
                m_gameManager.LoadMenu();
            }
        }

        public bool GetGameState() { return m_gameState; }

        public void SetWinState(bool val) { m_isWin = val; }
        public bool GetWinState() { return m_isWin; }
    }
}
