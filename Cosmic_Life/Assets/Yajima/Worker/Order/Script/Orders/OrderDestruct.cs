using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 自爆命令クラス
public class OrderDestruct : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Destruct");
    }
}
