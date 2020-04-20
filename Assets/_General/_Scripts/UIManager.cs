using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("Ani-Test");
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
