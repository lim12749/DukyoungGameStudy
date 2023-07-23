using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//이 마커를 이용하여 위치와 회전을 참조 하여 이동
//이 마커는 모든 세그먼트에 추가되어 각자의 위치를 참조하게 될
public class MarkerManager : MonoBehaviour
{
    public class Maker
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Quaternion OriginRot; 
        public Maker(Vector3 _pos, Quaternion _rot)
        {
            Position = _pos;
            Rotation = _rot;
        }
        public Maker(Vector3 _pos, Quaternion _rot, Quaternion _originRot)
        {
            Position = _pos;
            Rotation = _rot;
            OriginRot = _originRot;

        }
    }
     public List<Maker> makerList = new List<Maker>();

    public void FixedUpdate()
    {
        UpdateMarkerList();
    }
    //업데이트 마커 목록 마커는 기본적인 해드의 위치를 추적 기록합니다
    /// <summary>
    /// 마커는 업데이트마다 다음 마커의 위치를 기록하게되고 위치 회전을 기반으로 움직이게 될 것입니다 
    /// </summary>
    public void UpdateMarkerList()
    {
        var a = transform.localEulerAngles;
        var b = Quaternion.Euler(a);
        makerList.Add(new Maker(transform.position, transform.localRotation));
    }
    public void ClearMarkerList()
    {
        makerList.Clear();
        makerList.Add(new Maker(transform.position, transform.localRotation));

    }
}
