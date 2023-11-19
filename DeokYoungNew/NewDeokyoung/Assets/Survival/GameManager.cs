using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Survivor{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instace; //메모리에 올림
        public PoolManager pool;
        public Survivor.Player player;

        public float gameTime;
        public float maxGameTime = 2 * 10f;

        private void Awake()
        {
            instace = this;
        }
        private void Update()
        {
            gameTime += Time.deltaTime;
            if(gameTime >maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }
    }
}
