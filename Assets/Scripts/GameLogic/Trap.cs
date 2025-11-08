using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.GameOver();
        }
    }
}
