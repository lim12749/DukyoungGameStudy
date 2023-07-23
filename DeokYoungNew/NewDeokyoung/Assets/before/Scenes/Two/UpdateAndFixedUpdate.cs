using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAndFixedUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    /// <summary>
    /// 업데이트는 유니티에서 흔히 사용되는 함수 입니다.
    /// 우리가 사용중인 모든 스크립트에서 프레임당 한번씩 호출 됩니다.
    /// 정기적인 변경이나 조정이 필요한 거의 모든것을 사용
    /// 업데이트는 일정한 타임라인에 따라 호출되지 않습니다.
    /// 어떤 프레임의 처리 시간이 다음프레임보다 길면 
    /// 업데이트 호출 사이의 시간이 달라집니다.
    /// </summary>
    {
        ///모든 프레임 호출
        /// 정기적 업데이트에 사용
        /// 비 물리적인 객체 이동
        /// 간단한 타이머
        /// 입력
        Debug.Log("Update time :" + Time.deltaTime);
    }
    private void FixedUpdate()
        ///일정한 타임라인에 따라 호출
        ///호출사이의 시간이 동일
        ///호출된 직후 필요한 모든 물리 계산이 일어납니다
        ///리지드바디 물리 오브젝트에 영향을 미치는 요소는 fixed에 작성해야합니다.
        ///fixed 루프에서 물리 스크립팅을 하는경우 움직임에 물리적 힘을 사용하는것이 좋습니다.
    {
        Debug.Log("FixedUpdate time :" + Time.deltaTime);
    }
}
