using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 攻撃命令クラス
public class OrderAttack : Order {

    // 初期座標
    [SerializeField]
    protected Transform m_StartPoint;
    // 目的座標
    [SerializeField]
    protected Transform m_StopPoint;
    // 衝突判定オブジェクト
    [SerializeField]
    protected GameObject m_Collider;
    // 攻撃時間
    [SerializeField]
    private float m_AttackTime = 1.0f;

    // 時間
    private float m_Timer;
    // 攻撃が終了したか
    //private bool m_IsAttack;
    // 
    protected string m_OrderText = "Attack";

    // Use this for initialization
    public override void Start()
    {
        m_Collider.transform.position = m_StartPoint.position;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj)
    {
        base.StartAction(obj);
        // アクティブ状態に変更
        m_Collider.SetActive(true);
        // Tweenの移動
        m_Collider.transform.DOLocalMove(m_StopPoint.localPosition, m_AttackTime);
        m_Timer = 0.0f;
        m_IsEndOrder = false;
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        print(m_OrderText);

        //if (m_Timer >= m_AttackTime) return;
        m_Timer += deltaTime;
        if (m_Timer < m_AttackTime) return;

        // 攻撃判定を非アクティブ状態に変更する
        m_Collider.SetActive(false);
        //m_IsEndOrder = true;
        // イベントでの終了処理
        EndOrder(obj);
        var order = OrderStatus.ATTACK_HIGH;
        // 攻撃を終了する場合は、停止命令に変更する
        if (m_IsEndOrder)
        {
            order = OrderStatus.STOP;
            //m_IsEndOrder = false;
        }
        // 状態の変更
        ChangeOrder(obj, order);
    }

    public override void StopAction(GameObject obj)
    {
        m_IsEndOrder = true;
    }

    public override void EndAction(GameObject obj)
    {
        //base.EndAction();
        //m_IsEndOrder = false;
        if (m_Collider.activeSelf) m_Collider.SetActive(false);
        m_Collider.transform.position = m_StartPoint.position;
    }
}
