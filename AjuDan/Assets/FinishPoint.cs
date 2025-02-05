using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player mencapai finish point");
            FindAnyObjectByType<GameManager>().gameWin = true;
            gameManager.NextSceen();
        }
    }

}
