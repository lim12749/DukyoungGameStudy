using UnityEngine;

[DisallowMultipleComponent]
public class PickupItem : MonoBehaviour
{
    public ItemDefinition item;

    [Header("Visual (optional)")]
    public float rotateY = 45f;

    void Update()
    {
        if (rotateY != 0) transform.Rotate(0, rotateY * Time.deltaTime, 0, Space.World);
    }
}
