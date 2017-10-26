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
    protected float m_AttackTime = 1.0f;
    // 攻撃遅延時間
    [SerializeField]
    protected float m_DelayTime = 0.0f;
    // 戻る時間
    [SerializeField]
    protected float m_BackTime = 0.5f;

    // 時間
    protected float m_Timer;
    // 攻撃が終了したか
    private bool m_IsAttack = true;
    // 動かすオブジェクト
    protected GameObject m_MoveObject;
    // 
    protected string m_OrderText = "Attack";

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (m_Collider != null)
        {
            m_Collider.transform.position = m_StartPoint.position;
            m_MoveObject = m_Collider;
        }
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj);
        // アクティブ状態に変更
        m_Collider.SetActive(true);
        // Tweenの移動
        m_MoveObject.transform.DOLocalMove(m_StopPoint.localPosition, m_AttackTime);
        m_Timer = 0.0f;
        m_IsEndOrder = false;
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print(m_OrderText);

        //if (m_Timer >= m_AttackTime) return;
        m_Timer += deltaTime;
        if (m_Timer < m_AttackTime + m_DelayTime) return;

        // 攻撃戻り処理
        if (m_IsAttack)
        {
            // 戻る場合
            MoveObject();
            // 攻撃判定を非アクティブ状態に変更する
            if (m_Collider != null) m_Collider.SetActive(false);
            m_IsAttack = false;
        }
        //m_IsEndOrder = true;

        if (m_Timer < m_AttackTime + m_DelayTime + m_BackTime) return;

        // 攻撃終了処理
        // イベントでの終了処理
        EndOrder(obj);
        m_IsAttack = true;
        var order = m_OrderState;
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
        m_MoveObject.transform.position = m_StartPoint.position;
    }

    // 持っているオブジェクトの移動
    protected  virtual void MoveObject()
    {
        m_MoveObject.transform.DOLocalMove(m_StartPoint.localPosition, m_BackTime / 2);
    }
}
