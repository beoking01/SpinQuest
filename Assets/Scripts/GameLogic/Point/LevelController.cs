using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelConfig levelConfig;
    private TimeTracker timeTracker;
    private CheckRotation checkRotation;
    void Start()
    {
        timeTracker = GetComponent<TimeTracker>();
        checkRotation = GetComponent<CheckRotation>();
        // Kiểm tra an toàn trước khi gọi hàm
        timeTracker.StartTimer();
    }
    void Update()
    {
        if(GameManager.Instance.isWinGame())
        {
            LevelComplete();
        }
        
    }

    public void LevelComplete()
    {
        timeTracker.StopTimer();
        int starsEarned = new StarCalculate().CalculateStars(timeTracker.timeElapsed, checkRotation.countRotation, levelConfig);
        
        LevelManager.Instance.SaveStars(levelConfig.levelName, starsEarned);
        Debug.Log("Stars earned: " + starsEarned);
    }
}
