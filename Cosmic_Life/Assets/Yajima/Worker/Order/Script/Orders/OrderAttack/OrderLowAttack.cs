using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLowAttack : OrderAttack {

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_OrderText = "LowAttack";
    }

    //// Update is called once per frame
    //void Update () {

    //}

    //public override void Action(float deltaTime, GameObject obj)
    //{
    //    print("LowAttack");
    //}
}
