using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public void PlaytestScene1()
    {
        SceneManager.LoadScene(1);
    }

    public void PlaytestScene2()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}
