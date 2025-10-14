using UnityEngine;

[CreateAssetMenu(menuName = "LevelUp/UpgradeData")] 
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    //public Sprite icon;
    public UpgradeType upgradeType;
    public float value;
}

public enum UpgradeType
{
    MoveSpeed,
    MaxHP,
    AttackPower
}