using UnityEngine;
using System.Collections; // Cần thêm thư viện này để dùng Coroutine

public class CanvasCameraAssigner : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel; // Kéo cái Panel hoặc Text hướng dẫn vào đây

    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        
        // Tự động gán Camera nếu bị thiếu khi Build
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    void Start()
    {
        // Gọi hàm tắt hướng dẫn sau 3 giây kể từ khi game bắt đầu
        StartCoroutine(DisableTutorialAfterTime(3f));
    }

    IEnumerator DisableTutorialAfterTime(float delay)
    {
        // Chờ trong khoảng thời gian delay (3 giây)
        yield return new WaitForSeconds(delay);

        // Tắt đối tượng hướng dẫn
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
        }
        else
        {
            // Nếu bạn không kéo thả vào tutorialPanel, nó sẽ tự tắt chính cái Canvas này
            // gameObject.SetActive(false); 
            Debug.LogWarning("Chưa gán tutorialPanel trong Inspector!");
        }
    }
}