using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Audio.GeneratorInstance;

public class SpeedUpManager : MonoBehaviour
{
    public bool spedUp = false;
    public float speed = 2f;
    public RawImage fastForwardIcon;
    public void SpeedToggle()
    {
        spedUp = !spedUp;
        fastForwardIcon.color = spedUp ? Color.green : Color.white;
        Time.timeScale = spedUp ? speed : 1f;
    }
}
