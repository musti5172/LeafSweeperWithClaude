using System.Text;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private BagCapacity bag;
    [SerializeField] private LeafVacuum vacuum;
    [SerializeField] private PlayerMovement3D movement;
    [SerializeField] private Transform shopPoint;   // çöp kutusu
    [SerializeField] private float shopRange = 4f;

    private const int MaxLevel = 3;
    private readonly int[] levels = new int[4];
    private readonly string[] names =
    {
        "Çanta +10", "Vakum gücü", "Vakum menzili", "Yürüme hýzý"
    };

    private readonly StringBuilder sb = new StringBuilder();

    private int Cost(int index) => (levels[index] + 1) * 25;

    private void Update()
    {
        bool near = Vector3.Distance(transform.position, shopPoint.position) <= shopRange;

        if (!near)
        {
            UIManager.Instance.SetUpgradeText("");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) TryBuy(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryBuy(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryBuy(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TryBuy(3);

        ShowMenu();
    }

    private void TryBuy(int index)
    {
        if (levels[index] >= MaxLevel) return;
        if (!bag.TrySpend(Cost(index))) return;

        levels[index]++;

        switch (index)
        {
            case 0: bag.IncreaseCapacity(10); break;
            case 1: vacuum.UpgradePower(); break;
            case 2: vacuum.UpgradeRange(); break;
            case 3: movement.AddSpeed(0.7f); break;
        }
    }

    private void ShowMenu()
    {
        sb.Clear();
        sb.AppendLine("GELÝÞTÝRMELER");
        for (int i = 0; i < names.Length; i++)
        {
            sb.Append("[").Append(i + 1).Append("] ").Append(names[i])
              .Append("  Sv ").Append(levels[i]).Append("/").Append(MaxLevel);

            if (levels[i] >= MaxLevel) sb.AppendLine("  (MAX)");
            else sb.Append("  - ").Append(Cost(i)).AppendLine(" puan");
        }
        UIManager.Instance.SetUpgradeText(sb.ToString());
    }
}
