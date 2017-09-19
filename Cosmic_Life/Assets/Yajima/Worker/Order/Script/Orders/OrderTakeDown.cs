using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち下げ命令クラス
public class OrderTakeDown : Order {

    // 
    //private bool m_LiftObj;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj)
    {
        // 持ち上げたオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        // もし何も持っていなければ、返す
        if (liftObj.childCount == 0)
        {
            print("何も持っていません");
            return;
        }
        var stageObj = liftObj.GetChild(0).GetComponent<StageObject>();
        stageObj.transform.position -= Vector3.up * 1.0f;
        // ステージオブジェクトの親を初期化する
        stageObj.InitParent();
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        print("TakeDown");
    }
}
