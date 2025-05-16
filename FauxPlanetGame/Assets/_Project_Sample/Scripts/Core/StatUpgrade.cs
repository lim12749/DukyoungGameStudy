using UnityEngine;


[CreateAssetMenu(menuName="Upgrades/Stat")]
public class StatUpgrade : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string desc;
    public enum Type { Damage, FireRate, MoveSpeed, MaxHP }
    public Type type;
    public float value;   // % or flat
}