using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jelly
{
    public class GameManager : MonoBehaviour
    {
        private ActionSystem m_actionSystem;
        private Save.SaveInfo m_curProgress;

        private static GameManager _instance = null;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                InitGame();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void InitGame()
        {
            m_actionSystem = FindObjectOfType<ActionSystem>();

            LoadSave();
        }

        private void LoadSave()
        {
            m_curProgress = SaveLoad.LoadGame();
        }

        private void SaveGame()
        {
            SaveLoad.SaveGame(m_curProgress);
        }

        public void Play()
        {
            m_actionSystem.SetGameState(true);

            FindObjectOfType<EnemyGenerator>().UpdateSetting();
            FindObjectOfType<MainMenu>().UpdateUI();
        }

        public void Win()
        {
            m_curProgress.m_level++;
            m_actionSystem.SetGameState(false);

            FindObjectOfType<MainMenu>().UpdateUI();

            SaveGame();
        }

        public void ChangeMusicState()
        {
            bool newState = !m_curProgress.m_isMusic;

            AudioSource asc = GetComponent<AudioSource>();
            if (newState)
            {
                asc.Play();
                asc.loop = true;
            }
            else
            {
                asc.Stop();
            }

            m_curProgress.m_isMusic = newState;
            SaveGame();
        }

        public void ChangeSoundState()
        {
            m_curProgress.m_isSound = !m_curProgress.m_isSound;
            SaveGame();
        }

        public bool IsInAP() { return m_actionSystem.GetGameState(); }
        public int GetLevel() { return m_curProgress.m_level; }
        public bool IsMusicEnabled() { return m_curProgress.m_isMusic; }
        public bool IsSoundEnabled() { return m_curProgress.m_isSound; }
    }
}
