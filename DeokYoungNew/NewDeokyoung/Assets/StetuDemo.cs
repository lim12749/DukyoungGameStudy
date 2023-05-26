using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Option { NONE, HEILLING, DAMAGE}
public class StetuDemo : MonoBehaviour
{
    public Player player;
    public Option state = Option.NONE;

    public float T;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        T = T + Time.deltaTime;
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 입장");
            if (T > 2f)
            {
                switch (state)
                {
                    case Option.NONE:
                    
                        break;
                    case Option.HEILLING:
                        player.AddToDamage(10);
                        T = 0;
                        break;
                    case Option.DAMAGE:
                        player.AddToDamage(-10);
                        T = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
