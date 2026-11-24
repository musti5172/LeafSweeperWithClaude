using UnityEngine;

public class BagCapacity : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 20;
    private int currentCapacity;
    private int bagPoints;
    private int totalScore;

    //araþtýr
    public int TotalScore => totalScore;

    public bool TrySpend(int cost)
    {
        if (totalScore < cost) return false;

        totalScore -= cost;
        UIManager.Instance.UpdateScoreText(totalScore);
        return true;
    }

    public void IncreaseCapacity(int amount)
    {
        maxCapacity += amount;
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
    }

    //araþtýr
    public int MaxCapacity => maxCapacity;
    public int RemainingSpace => maxCapacity - currentCapacity;

    private void Start()
    {
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
        UIManager.Instance.UpdateScoreText(totalScore);
    }

    public bool IsFull()
    {
        return currentCapacity >= maxCapacity;
    }

    public void AddLeaf(int points)
    {
        if (currentCapacity >= maxCapacity) return;

        currentCapacity++;
        bagPoints += points;
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
    }

    public void EmptyBag()
    {
        totalScore += bagPoints;
        bagPoints = 0;
        currentCapacity = 0;

        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
        UIManager.Instance.UpdateScoreText(totalScore);
    }
}
