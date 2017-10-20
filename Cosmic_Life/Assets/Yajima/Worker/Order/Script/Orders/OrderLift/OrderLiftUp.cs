using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち上げ命令
public class OrderLiftUp : OrderLift {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        m_ActionObject = actionObj;
        // 持っているオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        if (liftObj.childCount != 0)
        {
            // 持ち上げる動作を行う
            m_LiftObject = liftObj.GetChild(0).gameObject;
            return;
        }

        // 持ち上げるオブジェクトの捜索
        FindLiftObject(obj, actionObj);
    }
}
