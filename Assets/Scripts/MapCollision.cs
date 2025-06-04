using UnityEngine;

public class MapCollision : MonoBehaviour
{
    GameController gameController;

    private void Awake()
    {
        gameController = Camera.main.GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Debug.Log("Reset");
            gameController.LoadScene();
        }
    }
}
