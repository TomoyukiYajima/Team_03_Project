using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち上げ命令クラス
public class OrderLift : Order {

    // 
    private enum LiftObjectNumber
    {
        PLAYER_LIFT_NUMBER = 1 << 0,
        OBJECT_LIFT_NUMBER = 1 << 1,
        ENEMY_LIFT_NUMBER  = 1 << 2,
    }

    // オブジェクトチェッカー
    [SerializeField]
    private ObjectChecker m_ObjectChecker;
    // 持ち上げオブジェクト確認用オブジェクト
    [SerializeField]
    protected CheckLiftObject m_CheckLiftObject;
    // 持ち上げポイント
    [SerializeField]
    private Transform m_LiftPoint;
    // 持ち上げたか
    protected bool m_IsLift = false;

    private bool m_isRotate = false;

    // 持ち上げるステージオブジェクト
    protected GameObject m_LiftObject;
    // 計算リスト
    private Dictionary<LiftObjectNumber, Action<int>> m_LiftMoves =
        new Dictionary<LiftObjectNumber, Action<int>>();

    // Use this for initialization
    public override void Start()
    {
        //m_LiftMoves.Add(LiftObjectNumber.PLAYER_LIFT_NUMBER, (value) => { });
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj)
    {
        // 持ち上げたオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        // もし何か持っていれば、返す
        if (liftObj.childCount != 0)
        {
            print("すでに物を持っています");
            return;
        }

        m_IsLift = false;
        m_isRotate = false;

        // プレイヤーとの距離を求める
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerLength = m_ObjectChecker.GetLength();
        if (player != null) playerLength = Vector3.Distance(player.transform.position, this.transform.position);

        if(playerLength >= m_ObjectChecker.GetLength())

        // プレイヤーが範囲外の場合、近くに何も持つものがない場合は、返す
        if (m_ObjectChecker.GetStageObjects().Count == 0 && playerLength >= m_ObjectChecker.GetLength())
        {
            print("持ち上げるものがありません");
            return;
        }

        // プレイヤーを持ち上げるオブジェクトに設定
        m_LiftObject = player;

        GameObject liftStageObj = null;
        // ステージオブジェクトとの距離を求める
        var objLength = m_ObjectChecker.GetLength();
        for (int i = 0; i != m_ObjectChecker.GetStageObjects().Count; ++i)
        {
            // 相手との距離を求める
            var length = Vector3.Distance(m_ObjectChecker.GetStageObjects()[i].transform.position, this.transform.position);
            if (objLength > length)
            {
                objLength = length;
                liftStageObj = m_ObjectChecker.GetStageObjects()[i];
            }
        }

        // ステージオブジェクトがプレイヤーより近い場合は、ステージオブジェクトを持ち上げる
        if (playerLength >= objLength) m_LiftObject = liftStageObj;

        // プレイヤーを持ち上げ状態に変更する
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Lift");

        if (m_IsLift) return;
        // 持ち上げられるかのチェック
        CheckLift(obj);
        // 移動
        Move(deltaTime, obj);
    }

    public override void EndAction()
    {
        m_LiftObject = null;
    }

    // 持ち上げられるかのチェックを行います
    protected void CheckLift(GameObject obj)
    {
        if (m_CheckLiftObject.IsCheckLift(m_LiftObject))
        {
            // 相手の持ち上げポイントを取得する
            var point = m_LiftObject.transform.Find("LiftPoint");
            float length = Mathf.Abs(point.position.y - m_LiftPoint.transform.position.y);
            m_LiftObject.transform.position += Vector3.up * length;
            // 持ち上げたオブジェクトを、ロボットの持つオブジェクトに変更する
            AddLiftObj(obj);
            return;
        }
    }

    // 移動
    protected void Move(float deltaTime, GameObject obj)
    {
        var dis = m_LiftObject.transform.position - this.transform.position;
        dis.y = 0.0f;
        var direction = Vector3.Normalize(dis);
        // オブジェクトの方向を向く
        if (!m_isRotate)
        {
            var dir = (
            new Vector3(m_LiftObject.transform.position.x, 0.0f, m_LiftObject.transform.position.z) -
            new Vector3(obj.transform.position.x, 0.0f, obj.transform.position.z)).normalized;
            obj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
            //obj.transform.rotation = Quaternion.FromToRotation(-obj.transform.forward, m_LiftObject.transform.position - this.transform.position);
            m_isRotate = true;
        }

        if (m_IsLift) return;
        // 移動
        float speed = 3.0f;
        obj.transform.position += obj.transform.forward * speed * deltaTime;
    }

    // オブジェクトの登録
    protected void AddLiftObj(GameObject obj)
    {
        var liftObj = obj.transform.Find("LiftObject");
        m_LiftObject.transform.parent = liftObj;
        // 剛体のキネマティックをオンにする
        var body = m_LiftObject.GetComponent<Rigidbody>();
        body.isKinematic = true;
        // 重力をオフにする
        body.useGravity = false;
        m_IsLift = true;
    }

    // プレイヤーのみを持ち上げる

    // ステージオブジェクトのみを持ち上げる

    // 敵を持ち上げる
}
