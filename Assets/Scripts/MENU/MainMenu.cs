
using UnityEngine;
using UnityEngine.UI;

namespace Jelly
{
    public class MainMenu : MonoBehaviour
    {
        private GameManager m_gameManager;

        [SerializeField] private Text m_textLevel;
        [SerializeField] private Text m_textLevelNext;
        [SerializeField] private GameObject m_textTapToPlay;
        [SerializeField] private GameObject m_buttonSetting;
        [SerializeField] private GameObject m_buttonPause;

        [SerializeField] private Slider m_progress;

        void Start()
        {
            m_gameManager = FindObjectOfType<GameManager>();

            UpdateUI();
        }

        public void UpdateUI()
        {
            m_textLevel.text = "";
            m_textLevelNext.text = "";

            int lvl = m_gameManager.GetLevel();

            m_textLevel.text += lvl;
            m_textLevelNext.text += lvl + 1;

            if (m_gameManager.IsInAP())
            {
                m_textTapToPlay.SetActive(false);
                m_buttonSetting.SetActive(false);

                m_buttonPause.SetActive(true);
            }
            else
            {
                m_textTapToPlay.SetActive(true);
                m_buttonSetting.SetActive(true);

                m_buttonPause.SetActive(false);
            }

            UpdateBar(0.0f);
        }

        public void UpdateBar(float ration)
        {
            m_progress.value = ration;
        }
    }
}