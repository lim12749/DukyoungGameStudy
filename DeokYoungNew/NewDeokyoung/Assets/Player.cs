using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp = 100;

    public void AddToDamage(float _vulue)
    {
        Debug.Log($"{_vulue}");
        Hp += _vulue;
    }
}
