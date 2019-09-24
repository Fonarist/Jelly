
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class ActionSystem : MonoBehaviour
    {
        private GameManager m_gameManager;
        private Field m_field;
        private Player m_player;

        [SerializeField] private float m_timeToCoin;
        private float m_timerToCoin;

        [SerializeField] private Text m_scoreText;
        private int m_score;

        private bool m_gameState;

        void Start()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            m_field = FindObjectOfType<Field>();
            m_player = FindObjectOfType<Player>();

            m_gameState = false;
            m_score = 0;

            m_scoreText.text = "";
        }

        void FixedUpdate()
        {

        }

        public void SetGameState(bool state) { m_gameState = state; }

        public bool GetGameState() { return m_gameState; }

        public void IncScore()
        {
            m_timerToCoin = m_timeToCoin;
            m_score++;

            m_scoreText.text = "";
            m_scoreText.text += m_score;

            int maxScore = Formulas.GetNeedCountCoin(m_gameManager.GetLevel());

            FindObjectOfType<MainMenu>().UpdateBar((float)m_score / maxScore);

            if (m_score >= maxScore)
            {
                Win();
            }
        }

        public void ClearScore()
        {
            m_timerToCoin = m_timeToCoin;
            m_score = 0;

            m_scoreText.text = "";
        }

        private void Win()
        {
            m_score = 0;
            m_scoreText.text = "";

            m_timerToCoin = 0;
            m_field.DeleteEnemies();
            m_player.SetDefaultPosition();
            m_gameManager.Win();
        }
    }
}
