using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jelly
{
    public class GlobalEvents : MonoBehaviour
    {
        public void GoToMenu()
        {
            FindObjectOfType<Player>().SetDefaultPosition();
            FindObjectOfType<Field>().DeleteEnemies();
            FindObjectOfType<ActionSystem>().SetGameState(false);
            FindObjectOfType<MainMenu>().UpdateUI();
        }

        public void Pause(bool state)
        {
            if(state)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void CleanSave()
        {
            SaveLoad.Clean();
        }

        public void ChangeMusicState()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.ChangeMusicState();
        }

        public void ChangeSoundState()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.ChangeSoundState();
        }
    }
}
