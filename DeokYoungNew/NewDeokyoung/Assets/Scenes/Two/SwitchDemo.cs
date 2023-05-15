using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDemo : MonoBehaviour
{
    public int intelliogentce = 5;

    void Greet()
    {
        switch (intelliogentce)
        {
            case 5:
                print("Why hello there good sir! Let me teach you about Trigonomertry");
                break; //점프문 case뒤에 오는 코드를 전부 무시하고 점프합니다.
            case 4:
                print("안녕 좋은 날이야");
                break; 
            case 3:
                print("뭐야");
                break; 
            case 2:
                print("Grog Smash!");
                break; 
            case 1:
                print("으어...");
                break;

            default: //if else 에서 else를 담당합니다 모든상황을 대응하게됩니다.
                break;
        }
    }
}
