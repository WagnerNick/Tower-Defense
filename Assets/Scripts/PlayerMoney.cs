using TMPro;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    [SerializeField] private TMP_Text moneyTxt;
    [SerializeField] private int startMoney = 650;
    public int money;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasSave())
        {
            SaveData data = SaveManager.Instance.Load();
            money = data.money;
        }
        else
        {
            money = startMoney;
        }
        moneyTxt.text = money.ToString();
    }

    public void ChangeMoney(int changeAmount, bool add)
    {
        money = add ? money + changeAmount : money - changeAmount;
        moneyTxt.text = money.ToString();
    }
}
