using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定方向を向くクラス
public class OrderLook : DirectionOrder {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, OrderDirection dir)
    {
        base.StartAction(obj, dir);

        // 方向の変更
        Worker worker = obj.GetComponent<Worker>();
        if (worker != null) worker.SetOrderDir(dir);
    }
}
