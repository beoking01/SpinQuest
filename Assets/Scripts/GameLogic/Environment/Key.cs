using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameManager.OpenDoor();
            SoundEventManager.PLaySound(SoundType.Dooropen);
            Destroy(gameObject);
        }
    }
}
