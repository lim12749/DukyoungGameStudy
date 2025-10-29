using UnityEngine;

public static class GroundRaycaster
{
    /// <summary> 스크린좌표에서 땅 레이어로 레이케스트해 월드 좌표를 얻음 
    
    public static bool TryGetGroundPoint(Camera _cam, Vector2 _screenPos, LayerMask _groundMask, out Vector3 _hitpoint)
    {
        var ray = _cam.ScreenPointToRay(_screenPos);
        if (Physics.Raycast(ray, out var hit, 500f, _groundMask, QueryTriggerInteraction.Ignore))
        {
            _hitpoint = hit.point;
            return true;
        }
        _hitpoint = default;
        return false;
        
    }
}
