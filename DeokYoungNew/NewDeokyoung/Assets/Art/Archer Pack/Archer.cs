using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {

    public Animator ani;
    public GameObject CameraObj;
    public GameObject weapon;

    public bool left = false;
    public bool right = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            left = false;
            right = false;
        }
        if (left == true)
        {
            CameraObj.transform.Rotate(Vector3.down, Time.deltaTime * 80f);
        }
        if (right == true)
        {
            CameraObj.transform.Rotate(Vector3.up, Time.deltaTime * 80f);
        }
      
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, Screen.height / 2 - 300, 100, 50), "idle"))
        {
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                ani.SetTrigger("idle");
                weapon.SetActive(true);
            }
        }
        if (GUI.Button(new Rect(100, Screen.height / 2 - 300, 100, 50), "run1"))
        {
            ani.SetTrigger("run1");
            weapon.SetActive(false);
        }
        if (GUI.Button(new Rect(200, Screen.height / 2 - 300, 100, 50), "run2"))
        {
            ani.SetTrigger("run2");
            weapon.SetActive(true);
        }
        if (GUI.Button(new Rect(300, Screen.height / 2 - 300, 100, 50), "attack"))
        {
            ani.SetTrigger("attack");
            weapon.SetActive(true);
        }
        if (GUI.Button(new Rect(400, Screen.height / 2 - 300, 100, 50), "trippleshot"))
        {
            ani.SetTrigger("trippleshot");
            weapon.SetActive(true);
        }
        if (GUI.Button(new Rect(500, Screen.height / 2 - 300, 100, 50), "backarrow"))
        {
            ani.SetTrigger("backarrow");
            weapon.SetActive(true);
        }
        if (GUI.Button(new Rect(600, Screen.height / 2 - 300, 100, 50), "kick"))
        {
            ani.SetTrigger("kick");
            weapon.SetActive(true);
        }
        if (GUI.Button(new Rect(700, Screen.height / 2 - 300, 100, 50), "death"))
        {
            ani.SetTrigger("death");
            weapon.SetActive(true);
        }

        if (GUI.RepeatButton(new Rect(Screen.width / 2 - 170, Screen.height / 2 + 200, 100, 50), "Camera Left"))
        {
            left = true;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 200, 100, 50), "Camera Reset"))
        {
            CameraObj.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (GUI.RepeatButton(new Rect(Screen.width / 2 + 70, Screen.height / 2 + 200, 100, 50), "Camera Right"))
        {
            right = true;
        }
    }
}
