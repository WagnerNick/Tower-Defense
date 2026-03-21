using TMPro;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text upgradeLabel;
    [SerializeField] private GameObject upgradeButton;

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
        RefreshUpgradeLabel();
    }

    public void Hide()
    {
        panel.SetActive(false);
        CurrentTower = null;
    }

    public void Refresh()
    {
        if (CurrentTower != null)
            RefreshUpgradeLabel();
    }

    void RefreshUpgradeLabel()
    {
        if (upgradeLabel == null || upgradeButton == null) return;

        if (CurrentTower.CanUpgrade)
        {
            UpgradeSO next = CurrentTower.NextUpgrade;
            upgradeLabel.text = $"{next.upgradeName}\n${next.cost}";
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeLabel.text = "Fully Upgraded";
            upgradeButton.SetActive(false);
        }
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
