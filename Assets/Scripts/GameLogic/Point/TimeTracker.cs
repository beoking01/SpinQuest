using UnityEngine;
using UnityEngine.Rendering;

public class TimeTracker : MonoBehaviour
{
    public float timeElapsed {get; private set;}
    public bool isRunning;

    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;
        }
    }
    public void StartTimer()
    {
        isRunning = true;
    }
    public void StopTimer()
    {
        isRunning = false;
    }
}
