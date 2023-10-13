using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Access type] [type] [class Name] : [parent class]
public class objectoriented : MonoBehaviour
{
    // Navigator system
    private string destination;
    private string route;
    int distance;

    //class method
    void SetCurrentLocation()
    {

    }
    void SetDestination(string _destination, string _route, int _distance)
    {
        destination = _destination;
        _route

    }
    void ModifyRouteToAvoid(string _value) { }
    void ModifyRouteToInclude(string _value) { }
    string GetRoute() {
        return destination;
    }
    void Idistance()
    {
        Debug.Log($"{destination}\n{route}\n{distance}");
    }
    void GetTimeToDestination() {}
    void TotalDistance() { }
}
