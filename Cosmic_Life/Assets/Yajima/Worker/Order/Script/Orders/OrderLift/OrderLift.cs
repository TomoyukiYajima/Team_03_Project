using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 持ち命令クラス
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
    // 持つオブジェクト確認用オブジェクト
    [SerializeField]
    protected CheckLiftObject m_CheckLiftObject;
    // 持つポイント
    [SerializeField]
    private Transform m_LiftPoint;
    // 持ったか
    protected bool m_IsLift = false;

    private bool m_isRotate = false;

    // 持つステージオブジェクト
    protected GameObject m_LiftObject;
    // 計算リスト
    private Dictionary<LiftObjectNumber, Action<int>> m_LiftMoves =
        new Dictionary<LiftObjectNumber, Action<int>>();

    // Use this for initialization
    //public override void Start()
    //{
    //    //m_LiftMoves.Add(LiftObjectNumber.PLAYER_LIFT_NUMBER, (value) => { });
    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        m_ActionObject = actionObj;
        // 持っているオブジェクトを、元の親に戻す
        var liftObj = obj.transform.Find("LiftObject");
        // もし何か持っていれば、返す
        if (liftObj.childCount != 0)
        {
            print("すでに物を持っています");
            return;
        }

        // オブジェクトの捜索
        FindLiftObject(obj, actionObj);

        // リフトクラスを継承した子クラスのオブジェクトチェック関数を呼ぶ
        // m_LiftCheck[checkNumber].CheckObject(obj);

        // プレイヤーを持つ状態に変更する
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Lift");

        if (m_IsLift) return;
        // 持てるかのチェック
        CheckLift(obj);
        // 移動
        Move(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        //m_IsLift = false;
        m_LiftObject = null;
        m_CheckLiftObject.ReleaseObject();
    }

    // 持てるかのチェックを行います
    protected void CheckLift(GameObject obj)
    {
        if (m_CheckLiftObject.IsCheckLift(m_LiftObject))
        {
            // 相手の持つポイントを取得する
            var point = m_LiftObject.transform.Find("LiftPoint");
            float length = Mathf.Abs(point.position.y - m_LiftPoint.transform.position.y);
            m_LiftObject.transform.position += Vector3.up * length;
            // 持つオブジェクトを、ロボットの持つオブジェクトに変更する
            AddLiftObj(obj);
            return;
        }
    }

    // 移動
    protected void Move(float deltaTime, GameObject obj)
    {
        //if(m_LiftObject == null)
        //{
        //    EndOrder(obj);
        //    return;
        //}

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
        // 固定しているステータスを解除
        body.constraints = RigidbodyConstraints.None;
        m_IsLift = true;
    }

    // 持ち上げるオブジェクトの捜索
    protected void FindLiftObject(GameObject obj, GameObject actionObj)
    {
        m_IsLift = false;
        m_isRotate = false;

        // 参照するオブジェクトがある場合
        if (actionObj != null)
        {
            // 見ているものを持つオブジェクトに変更する
            m_LiftObject = actionObj;
            return;
        }

        // プレイヤーとの距離を求める
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerLength = m_ObjectChecker.GetLength();
        if (player != null) playerLength = Vector3.Distance(player.transform.position, this.transform.position);

        if (playerLength >= m_ObjectChecker.GetLength())
        {
            // プレイヤーが範囲外の場合、近くに何も持つものがない場合は、返す
            if (m_ObjectChecker.GetStageObjects().Count == 0)
            {
                print("持ち上げるものがありません");
                // 空の状態に遷移
                //ChangeOrder(obj, OrderStatus.NULL);
                EndOrder(obj);
                return;
            }
        }

        // プレイヤーを持つオブジェクトに設定
        m_LiftObject = player;

        GameObject liftStageObj = m_ObjectChecker.GetStageObjects()[0];
        // ステージオブジェクトとの距離を求める
        //var objLength = m_ObjectChecker.GetLength();
        var objLength = Vector3.Distance(liftStageObj.transform.position, this.transform.position);
        for (int i = 1; i != m_ObjectChecker.GetStageObjects().Count; ++i)
        {
            // 相手との距離を求める
            var length = Vector3.Distance(m_ObjectChecker.GetStageObjects()[i].transform.position, this.transform.position);
            if (objLength > length)
            {
                objLength = length;
                liftStageObj = m_ObjectChecker.GetStageObjects()[i];
            }
        }

        // ステージオブジェクトがプレイヤーより近い場合は、ステージオブジェクトを持つ
        if (playerLength >= objLength) m_LiftObject = liftStageObj;

        if (m_LiftObject == null)
        {
            // 
            m_LiftObject = liftStageObj;
        }
    }

    // プレイヤーのみを持ち上げる

    // ステージオブジェクトのみを持ち上げる

    // 敵を持ち上げる
}
