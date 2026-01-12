using System.Collections;
using UnityEngine;

public class RotationMap : MonoBehaviour
{
    private Quaternion targetRotation;
    private float startPoint = 0.0f;
    private bool delay = true;

    void Start()
    {
        targetRotation = transform.rotation;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && delay)
        {
            startPoint -= 90;
            targetRotation = Quaternion.Euler(0, 0, startPoint);
            StartCoroutine(waitTime());
        }
        else if (Input.GetKeyDown(KeyCode.E) && delay)
        {
            startPoint += 90;
            targetRotation = Quaternion.Euler(0, 0, startPoint);
            StartCoroutine(waitTime());
        }
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );
    }
    IEnumerator waitTime()
    {
        delay = false;
        yield return new WaitForSeconds(0.8f);
        delay = true;
    }
}
