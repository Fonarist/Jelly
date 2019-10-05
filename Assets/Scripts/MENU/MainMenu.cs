
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class MainMenu : MonoBehaviour
    {
        private GameManager m_gameManager;
        private ActionSystem m_actionSystem;

        [SerializeField] private Text m_textLevel;
        [SerializeField] private GameObject m_textTapToPlay;
        [SerializeField] private GameObject m_buttonPause;
        [SerializeField] private GameObject m_panelPause;
        [SerializeField] private GameObject m_finalPanel;
        [SerializeField] private GameObject m_finalPanelTextWin;
        [SerializeField] private GameObject m_finalPanelTextLose;

        [SerializeField] private Slider m_progress;
        [SerializeField] private Text m_money;

        void Start()
        {
            m_gameManager = FindObjectOfType<GameManager>();
            m_actionSystem = FindObjectOfType<ActionSystem>();

            UpdateUI();
        }

        public void UpdateUI()
        {
            m_progress.gameObject.SetActive(true);
            m_textLevel.text = "";
            m_textLevel.text += "Level " + m_gameManager.GetLevel();

            if (m_gameManager.IsInAP())
            {
                m_textTapToPlay.SetActive(false);

                m_buttonPause.SetActive(true);
            }
            else
            {
                m_textTapToPlay.SetActive(true);

                m_buttonPause.SetActive(false);
            }

            UpdateBar(0.0f);
            UpdateMoney();
        }

        public void UpdateBar(float ration)
        {
            if (ration < 0.0f)
            {
                m_progress.gameObject.SetActive(false);
            }
            else
            {
                m_progress.value = ration;
            }
        }

        public void UpdateMoney()
        {
            m_money.text = "";
            m_money.text += m_gameManager.GetMoney();
        }

        public void EnableFinalPanel()
        {
            m_panelPause.SetActive(false);
            m_finalPanel.SetActive(true);

            if(m_actionSystem.GetWinState())
            {
                m_finalPanelTextLose.SetActive(false);
                m_finalPanelTextWin.SetActive(true);
            }
            else
            {
                m_finalPanelTextWin.SetActive(false);
                m_finalPanelTextLose.SetActive(true);
            }
        }
    }
}