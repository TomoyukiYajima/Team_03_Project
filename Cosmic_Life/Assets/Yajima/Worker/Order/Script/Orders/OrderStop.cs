using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStop : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Stop");
    }
}
