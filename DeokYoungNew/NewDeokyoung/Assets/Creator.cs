using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Seven
{
    public class Creator : MonoBehaviour{

        GameObject Player;
        private void Start()
        {
            PlayerBace player1 = new SowrdMan();
            Debug.Log($"{player1.GetHp()}") ;
            Debug.Log($"{player1.GetAttack()}");
            
        }
        public void CreatePlayer()
        {
            var a= Instantiate(Player);
        }
    }

}
