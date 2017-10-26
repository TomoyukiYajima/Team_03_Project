using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderJump : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj);

        // 上に移動量の加算
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.up * 4.0f);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        //base.Action(deltaTime, obj);
    }
}
