using UnityEngine;

public class CheckRotation : MonoBehaviour
{
    public int countRotation {get; private set;}
    void Start()
    {
        countRotation = 0;
    }

    void Update()
    {
        if (GameManager.Instance.IsPlay())
        {
            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                countRotation++;
            }
        }
    }
}