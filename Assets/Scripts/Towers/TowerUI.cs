using UnityEngine;

public class TowerUI : MonoBehaviour
{
    public static TowerUI instance;

    void Awake() => instance = this;

    public void Show(Tower tower)
    {

    }

    public void Hide()
    {

    }
}
