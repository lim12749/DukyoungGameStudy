using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    public float sensitivityX = 150f;
    public float sensitivityY = 1.5f;
    public float minY = -60f;
    public float maxY = 70f;

    private float rotationX; // 위/아래
    private float rotationY; // 좌/우
    private PlayerInputReader input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputReader>();
    }

    void Update()
    {
        rotationY += input.MouseX * sensitivityX * Time.deltaTime;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minY, maxY);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
