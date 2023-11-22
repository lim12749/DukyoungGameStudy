using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor{
    public class RePosition : MonoBehaviour
    {
        Collider2D colls;
        private void Awake()
        {
            colls = GetComponent<Collider2D>();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("Area"))
                return;

            Debug.Log("되냐?");
            Vector3 playerPosition = GameManager.instace.player.transform.position;
            Vector3 myPosition = transform.position;
            //절대값을 붙여 양수로 만듬
            float diffX = Mathf.Abs(playerPosition.x - myPosition.x);
            float diffY = Mathf.Abs(playerPosition.y - myPosition.y);

            Vector3 playerdir = GameManager.instace.player.inputVec;
            //3항 연산 (조건) ? (참) : (거짓)
            float dirX = playerdir.x < 0 ? -1 : 1;
            float dirY = playerdir.y < 0 ? -1 : 1;

            switch(transform.tag)
            {
                case "Ground":
                    //축상으로 x가 더큰경우
                    if (diffX > diffY)
                        //두칸씩 뛰어서 40임
                        transform.Translate(Vector3.right * dirX * 40);
                    else if (diffX < diffY)
                        transform.Translate(Vector3.up * dirY * 40);
                    break;
                case "Enemy":
                    if (colls.enabled)
                    {
                        //플레이어의 이동방햐ㅐㅇ에 따라 맞은편에서 등장하도록 이동
                        transform.Translate(playerdir * 20+new Vector3(Random.Range(-3f,3f), Random.Range(-3f, 3f),0f));
                    }
                    break;
            }
        }
    }
}
