public class StarCalculate
{
    public int CalculateStars(float timeElapsed, int countRotation, LevelConfig levelConfig)
    {
        int star = 1;
        if (timeElapsed <= levelConfig.starTimer || levelConfig.starTimer == 0)
        {
            star++;
        }
        if (countRotation <= levelConfig.useageRotationStar || levelConfig.useageRotationStar == 0)
        {
            star++;
        }
        return star;
    }
}

