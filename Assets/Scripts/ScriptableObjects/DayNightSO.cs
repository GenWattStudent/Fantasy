using UnityEngine;

[CreateAssetMenu(fileName = "Lightning Preset", menuName = "ScriptableObjects/Ligthing Preset")]
public class DayNightSO : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
}
