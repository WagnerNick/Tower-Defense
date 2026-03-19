using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TD/Wave List")]
public class WaveDataSO : ScriptableObject
{
    public List<WaveDef> waves;
}

[System.Serializable]
public class WaveDef
{
    public string label;
    public List<GroupDef> groups;
}

[System.Serializable]
public class GroupDef
{
    public BalloonTypeSO balloonType;
    public int count;
    public float startDelay;
    public float interval;
}
