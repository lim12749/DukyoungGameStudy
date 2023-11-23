using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace MainGame
{
public class Crater : FauxGravitybody
{    
        public float shrinkSpeed = 0.05f;

        bool isVanishing = false;

        void Update()
        {
            if (isVanishing)
                return;

            transform.localScale *= 1f - shrinkSpeed * Time.deltaTime;

            if (transform.localScale.x <= .5f)
            {
                isVanishing = true;
                GetComponent<Animator>().SetTrigger("Vanish");
                Destroy(gameObject, .3f);
            }
        }

    }
}
