using TMPro;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    [SerializeField] private TMP_Text moneyTxt;
    [SerializeField] private int startMoney = 650;
    public int money;

    private void Awake()
    {
        Instance = this;
        money = startMoney;
        moneyTxt.text = money.ToString();
    }

    public void ChangeMoney(int changeAmount, bool add)
    {
        money = add ? money += changeAmount : money -= changeAmount;
        moneyTxt.text = money.ToString();
    }
}
