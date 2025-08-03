using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            AudioManager.Instance.PlayMusic("MainMenuMusic");
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            AudioManager.Instance.PlayMusic("LevelMusic");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit();
    }
}
