using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPullOut : OrderLift {

    // 後ずさりしたか
    private bool m_IsBackMove = false;
    // 後ずさりする時間
    private float m_BackTime;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    //public override void StartAction(GameObject obj)
    //{
    //    //base.StartAction(obj);
    //    // 持ち上げたオブジェクトを取得する
    //    var liftObj = obj.transform.Find("LiftObject");
    //    // もし何か持っていれば返す
    //    if (liftObj.childCount != 0)
    //    {
    //        print("すでに物を持っています");
    //        return;
    //    }

    //    // 
    //}

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("PullOut");

        if (m_IsBackMove) return;

        if (m_IsLift)
        {
            var speed = 1.0f;
            obj.transform.position += -obj.transform.forward * speed * deltaTime;
            m_BackTime += deltaTime;
            // 後ずさりが完了したか
            if (m_BackTime < 1.0f) return;
            m_IsBackMove = true;
            return;
        }

        // 持ち上げられるかのチェック
        if (m_CheckLiftObject.IsCheckLift(m_LiftObject))
        {
            AddLiftObj(obj);
            return;
        }

        // 移動
        Move(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);

        m_BackTime = 0.0f;
        m_IsBackMove = false;
    }
}
