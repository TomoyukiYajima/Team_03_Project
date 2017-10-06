using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderAllStop : Order {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj);

        //StopAction
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の変更)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.stopOrder(); });

        // 空の状態に変更する
        //ChangeOrder(obj, OrderStatus.NULL);
        EndOrder(obj);
    }
}
