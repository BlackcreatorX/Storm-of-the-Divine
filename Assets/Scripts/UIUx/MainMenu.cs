using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("Start Game!");
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

}
