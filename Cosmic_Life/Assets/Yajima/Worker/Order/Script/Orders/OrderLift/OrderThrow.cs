using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderThrow : Order {

    // 移動量
    private Vector3 m_Velocity = Vector3.forward;
    // リフトムーブマネージャ
    private LiftMoveManager m_LiftManager;

    private float m_Power = 50.0f;

    // 移動量コンテナ
    private Dictionary<OrderDirection, Vector3> m_Velocities =
        new Dictionary<OrderDirection, Vector3>();

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        // 追加
        m_Velocities[OrderDirection.FORWARD] = Vector3.zero;
        m_Velocities[OrderDirection.BACKWARD] = Vector3.forward * 2;
        m_Velocities[OrderDirection.LEFT] = Vector3.left * 0.8f;
        m_Velocities[OrderDirection.RIGHT] = Vector3.right * 0.8f;
        m_Velocities[OrderDirection.UP] = Vector3.up * 0.8f;
        m_Velocities[OrderDirection.DOWN] = Vector3.down * 0.8f;

        // マネージャの追加
        m_LiftManager = gameObject.GetComponent<LiftMoveManager>();
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj = null)
    {
        base.StartAction(obj, actionObj);

        // 何も持っていなければ、空の命令に変更
        if (!m_LiftManager.CheckLiftObject(obj))
        {
            ChangeOrder(obj, OrderStatus.NULL);
            return;
        }

        print("投げ命令");

        // 親子関係を解除する
        m_LiftManager.ReleaseObject();
        
        // 方向によってベクトルの加算を行う
        m_Velocity = (obj.transform.forward + m_Velocities[m_Dir]).normalized;
        GameObject obj2 = m_LiftManager.GetLiftObject();
        Rigidbody body = m_LiftManager.GetLiftObject().GetComponent<Rigidbody>();
        body.AddForce(m_Velocity * m_Power, ForceMode.Impulse);

        // 空の状態に遷移
        ChangeOrder(obj, OrderStatus.NULL);
    }
}
