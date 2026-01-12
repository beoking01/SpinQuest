using UnityEngine;

public class Door : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;
    void Start()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (gameManager.IsOpen())
        {
            animator.SetBool("isOpen", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager.IsOpen())
            {
                gameManager.WinGame();
            }
        }
    }
}
