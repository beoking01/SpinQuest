using UnityEngine;

public class BoxWood : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;
    private GameObject player;
    public float deadlyVelocity = 1f; // Ngưỡng vận tốc gây chết, có thể điều chỉnh trong Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        player = GameObject.FindWithTag("Player");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu va chạm với player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision");
            // Kiểm tra vận tốc rơi (theo trục y)
            float fallVelocity = rb.linearVelocity.y;

            // Nếu vận tốc rơi lớn hơn ngưỡng cho phép
            if (fallVelocity > deadlyVelocity || fallVelocity < -deadlyVelocity)
            {
                if (player.GetComponent<PlayerMove>().IsGrounded())
                {
                    // Gọi GameOver
                    gameManager.GameOver();
                }
                
            }
        }
    }
}
