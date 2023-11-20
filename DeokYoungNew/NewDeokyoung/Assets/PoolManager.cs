using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Survivor{
    public class PoolManager : MonoBehaviour
    {
        //프리팹 보관 변수
        public GameObject[] Prefabs;
        //풀담당 리스트
        List<GameObject>[] Enemys;

        private void Awake()
        {
            Enemys = new List<GameObject>[Prefabs.Length];

            for (int index = 0; index < Enemys.Length; index++)
            {
                Enemys[index]= new List<GameObject> ();//초기화
            }
        }
        public GameObject Get(int index)
        {
            GameObject select = null;
            
            foreach(GameObject item in Enemys[index])
            {
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }
            if(!select)
            {
                select = Instantiate(Prefabs[index],transform);
                Enemys[index].Add(select);
            }
            return select;
        }
    }
}
