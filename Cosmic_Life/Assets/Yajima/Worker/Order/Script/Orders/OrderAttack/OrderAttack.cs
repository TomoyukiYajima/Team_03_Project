using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 攻撃命令クラス
public class OrderAttack : Order {

    // 目的座標
    [SerializeField]
    protected Transform m_StopPoint;
    // 衝突判定
    [SerializeField]
    protected GameObject m_Collider;
    // 攻撃速度
    [SerializeField]
    private float m_AttackSpeed = 1.0f;
    // 初期の衝突判定位置
    protected Vector3 m_InitAttackPosition;

    // 時間
    private float m_Timer;
    // 攻撃が終了したか
    //private bool m_IsAttack;
    // 
    protected string m_OrderText = "Attack";

    // Use this for initialization
    public override void Start()
    {
        //m_InitAttackPosition = this.transform.position;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj)
    {
        base.StartAction(obj);

        // 初期位置
        m_InitAttackPosition = m_Collider.transform.position;
        //print(m_Collider.transform.position);

        m_Collider.SetActive(true);
        // Tweenの移動
        m_Collider.transform.DOMove(m_StopPoint.position, m_AttackSpeed);
        m_Timer = 0.0f;
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        print(m_OrderText);

        if (m_Timer >= m_AttackSpeed) return;
        m_Timer += Time.deltaTime;
        if (m_Timer < m_AttackSpeed) return;
        // 攻撃判定を非アクティブ状態に変更する
        m_Collider.SetActive(false);
        m_Collider.transform.position = m_InitAttackPosition;
        m_IsEndOrder = true;
    }

    public override void EndAction()
    {
        base.EndAction();

        //print(m_Collider.transform.position);

        //m_Collider.SetActive(false);
        m_Collider.transform.position = m_InitAttackPosition;
        //m_Collider.transform.position = m_InitAttackPosition;
    }
}
