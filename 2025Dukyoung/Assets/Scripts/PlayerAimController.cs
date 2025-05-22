using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    public float sensitivityX = 150f;
    public float sensitivityY = 1.5f;
    public float minY = -60f;
    public float maxY = 70f;

    float rotationX, rotationY;
    private PlayerInputReader input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputReader>();
    }


    void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -60f, 70f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);

        Debug.Log($"X: {rotationX}, Y: {rotationY}");
    }

}
