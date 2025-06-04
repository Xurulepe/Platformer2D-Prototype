using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Level1");
    }
}
