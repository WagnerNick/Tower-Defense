using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text upgradeLabel;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private TMP_Dropdown targetDropdown;

    public Tower CurrentTower { get; private set; }

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);

        targetDropdown.ClearOptions();
        targetDropdown.AddOptions(new List<string>
        {
            "First", "Last", "Close", "Strong"
        });
        targetDropdown.onValueChanged.AddListener(OnTargetModeChanged);
    }

    public void Show(Tower tower)
    {
        CurrentTower = tower;
        panel.SetActive(true);
        RefreshUpgradeLabel();

        targetDropdown.SetValueWithoutNotify((int)tower.TargetMode);
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

    void OnTargetModeChanged(int index)
    {
        CurrentTower?.SetTargetMode((TargetMode)index);
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
