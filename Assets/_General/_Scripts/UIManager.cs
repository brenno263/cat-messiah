using UnityEngine;
using UnityEngine.SceneManagement;

namespace _General._Scripts
{
    public class UIManager : MonoBehaviour
    {
        public void Level1()
        {
            SceneManager.LoadScene("FireDetectionTesting");
        }

        public void Level2()
        {
            SceneManager.LoadScene("Ani-Test");
        }

        public void Level3()
        {
            SceneManager.LoadScene("Ani-Test");
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
