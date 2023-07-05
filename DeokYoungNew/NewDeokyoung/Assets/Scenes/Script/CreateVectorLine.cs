using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateVectorLine : MonoBehaviour
{
    public GameObject LineObj;
    public TMP_InputField x;
    public TMP_InputField y;
    public TMP_InputField z;

    public void CreateLineButton()
    {
        var _x = float.Parse(x.text);
        var _y = float.Parse(y.text);
        var _z = float.Parse(z.text);

       //Onbutton Click
        var line = Instantiate(LineObj);

        var render = line.GetComponent<LineRenderer>();
        render.positionCount = 2;
        render.SetPosition(0, new Vector3(0f,0f,0f));
        render.SetPosition(1, new Vector3(_x,_y,_z));
    }
}
