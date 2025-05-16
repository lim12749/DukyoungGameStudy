using UnityEngine;

public class PlayerStates : MonoBehaviour
{
   public float baseDamage = 10, baseFireRate = 1, baseSpeed = 8;
    public int   maxHP = 100;

    float dmgMul = 1, rateMul = 1, spdMul = 1;

    public float Damage     => baseDamage   * dmgMul;
    public float FireInterval   => baseFireRate * rateMul;
    public float MoveSpeed
    {
        get { return baseSpeed * spdMul; }
    }

    public void ApplyUpgrade(StatUpgrade up)
    {
        switch (up.type)
        {
            case StatUpgrade.Type.Damage:    dmgMul += up.value; break;
            case StatUpgrade.Type.FireRate:  rateMul += up.value; break;
            case StatUpgrade.Type.MoveSpeed: spdMul += up.value; break;
            case StatUpgrade.Type.MaxHP:     maxHP  += (int)up.value; break;
        }
        Debug.Log($"[Stats] {up.upgradeName} 적용! → Damage {Damage}, FireRate {1/FireInterval:F2}/s, Speed {MoveSpeed}");
        
    }
}
