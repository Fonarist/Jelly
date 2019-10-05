using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jelly
{
    public class GameManager : MonoBehaviour
    {
        private MainMenu m_mainMenu;
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
            m_mainMenu = FindObjectOfType<MainMenu>();

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

        public void LoadAP()
        {
            FindObjectOfType<CameraMovement>().StartMove();
            m_mainMenu.UpdateUI();
        }

        public void LoadMenu()
        {
            if(m_actionSystem.GetWinState())
            {
                m_curProgress.m_level++;
                SaveGame();
            }

            Time.timeScale = 1;
            m_mainMenu.UpdateUI();
            FindObjectOfType<Player>().SetDefault();
            FindObjectOfType<Field>().Reset();
            FindObjectOfType<CameraMovement>().SetDefault();

        }

        public void AddMoney(int val)
        {
            m_curProgress.m_money += val;

            m_mainMenu.UpdateMoney();

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

        public void ChangeVibroState()
        {
            m_curProgress.m_isVibro = !m_curProgress.m_isVibro;
            SaveGame();
        }

        public bool IsInAP() { return m_actionSystem.GetGameState(); }
        public int GetLevel() { return m_curProgress.m_level; }
        public int GetMoney() { return m_curProgress.m_money; }
        public bool IsMusicEnabled() { return m_curProgress.m_isMusic; }
        public bool IsVibroEnabled() { return m_curProgress.m_isVibro; }
    }
}
