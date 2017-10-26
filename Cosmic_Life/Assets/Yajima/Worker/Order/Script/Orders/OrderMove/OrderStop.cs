using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 停止命令クラス
public class OrderStop : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Stop");
    }
}
