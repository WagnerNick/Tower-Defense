using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance;
    [SerializeField] private GameObject panel;
    public Tower CurrentTower { get; private set; }

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(Tower tower)
    {
        CurrentTower = tower;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        CurrentTower = null;
    }

    public void UpgradeButton()
    {
        CurrentTower?.Upgrade();
    }

    public void SellButton()
    {
        CurrentTower?.Sell();
        Hide();
    }
}
