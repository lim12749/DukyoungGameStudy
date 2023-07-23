using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfStatements : MonoBehaviour
{
    public GameObject Target;

    float coffeeTeamperature = 85.0f;
    float hotLimitTeamperature = 70.0f;
    float coldLimitTemperature = 40.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {//TemperatureTest();
            TargetOnOff();
        }
        coffeeTeamperature -= Time.deltaTime * 5f;
    }
    private void TargetOnOff()
    {
        var a = Target.activeSelf;
        if (Target.activeSelf)
        {
            Target.SetActive(false);
        }
        else if(!Target.activeSelf)
        {
            Target.SetActive(true);
        }
        //Target.SetActive(!a);
    }
    private void TemperatureTest()
    {
        if (coffeeTeamperature > hotLimitTeamperature)
        {
            print("Coffee is too hot.");
        }
        else if (coffeeTeamperature < coldLimitTemperature)
        {
            print("Coffee is too Cold.");
        }
        else
        {
            print("Coffee is Just Right");
        }
    }
}
