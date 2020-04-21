using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _General._Scripts
{
    public class UIManager : MonoBehaviour
    {
        public List<string> levels;
        
        public void Level1()
        {
            SceneManager.LoadScene(levels[0]);
        }

        public void Level2()
        {
            SceneManager.LoadScene(levels[1]);
        }

        public void Level3()
        {
            SceneManager.LoadScene(levels[2]);
        }

        public void NextLevel()
        {
            int level = PlayerPrefs.GetInt("level");
            
            if(level < 2) SceneManager.LoadScene(levels[level + 1]);
        }

        public void RestartLevel()
        {
            int level = PlayerPrefs.GetInt("level");
            
            if(level < 3) SceneManager.LoadScene(levels[level]);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Credits()
        {
            SceneManager.LoadScene("Credits");
        }
    }
}
