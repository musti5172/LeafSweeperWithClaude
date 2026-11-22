using UnityEngine;

public class BagCapacity : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 20;
    private int currentCapacity;

    public int MaxCapacity => maxCapacity;
    public int RemainingSpace => maxCapacity - currentCapacity;

    private void Start()
    {
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
    }

    public bool IsFull()
    {
        return currentCapacity >= maxCapacity;
    }

    public void AddLeaf()
    {
        if (currentCapacity >= maxCapacity) return;

        currentCapacity++;
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
    }

    public void EmptyBag()
    {
        currentCapacity = 0;
        UIManager.Instance.UpdateLeafCountText(currentCapacity, maxCapacity);
    }
}
