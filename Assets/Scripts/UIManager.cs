using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI leafCountText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateLeafCountText(int current, int max)
    {
        leafCountText.text = current + "/" + max;
    }
}