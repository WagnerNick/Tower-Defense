using UnityEngine;

[CreateAssetMenu(fileName = "BalloonType", menuName = "TD/Balloon Type")]
public class BalloonTypeSO : ScriptableObject
{
    [Header("Identity")]
    public string balloonName = "Red";
    public Color color = Color.red;

    [Header("Stats")]
    public int health = 1;
    public float speed = 1f;
    public float scale = 1f;

    [Header("On Pop")]
    //null = death on pop
    public BalloonTypeSO popsInto;
}
