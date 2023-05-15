using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherClass
{
    public int apples;
    public int bananas;

    private int stealer;
    private int sellotape;

    protected string Shop;

    internal float SmapleInternal;

    public void FruitMachine(int a,int b)
    {
        int answer;
        answer = a + b;
        Debug.Log("Fruit Total" + answer);
    }
    private void OfficeSort(int a, int b)
    {
        int answer;
        answer = a + b;
        Debug.Log("Office Supplies total: " + answer);
    }
}

public class ScopeAndAccessModifiers : MonoBehaviour
{
    public int Alpha = 5;

    private int beta = 0;
    private int gamma = 5;

    private AnotherClass myOtherClass;

    private void Start()
    {
        Alpha = 30;

        myOtherClass = new AnotherClass();
        myOtherClass.FruitMachine(Alpha, myOtherClass.apples);
    }
    void Example(int pens, int crayons)
    {
        int answer;
        answer = pens * crayons * Alpha;
        Debug.Log(answer);
    }


    void Update()
    {
        Debug.Log("Alpha is set to: " + Alpha);
    }
}
