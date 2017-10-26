using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち下げ命令クラス
public class OrderTakeDown : Order {

    // 置くポイント
    [SerializeField]
    private Transform m_Point;

    // 
    //private bool m_LiftObj;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
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
        // 相手の持ち上げポイントを取得する
        var point = stageObj.transform.Find("LiftPoint");
        float length = Mathf.Abs(point.position.y - m_Point.transform.position.y);
        stageObj.transform.position += Vector3.down * length;
        //stageObj.transform.position -= Vector3.up * 1.0f;
        // 剛体のキネマティックをオフにする
        var body = stageObj.GetComponent<Rigidbody>();
        body.isKinematic = false;
        // 重力をオンにする
        body.useGravity = true;
        // ステージオブジェクトの親を初期化する
        stageObj.InitParent();
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("TakeDown");
    }
}
