using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemyEvent {

    //命令列挙
    [SerializeField]
    protected EnemyStatus m_OrderState = EnemyStatus.None;
    // 命令格納コンテナ
    protected Dictionary<EnemyStatus, System.Action<float, Enemy>> m_Orders =
        new Dictionary<EnemyStatus, System.Action<float, Enemy>>();

    //命令リスト
    protected EnemyStateList m_StateList;

    // 命令実行時間
    protected float m_StateTimer = 0.0f;


    //巡回のポイント
    [SerializeField, Tooltip("巡回のポイントを設定する")]
    protected Transform[] m_RoundPoints;

    [SerializeField]
    protected int m_Hp;

    //public NavMeshAgent m_Agent;

    ////現在の巡回ポイントのインデックス
    //private int m_CurrentPatrolPointIndex = -1;

    ////見える距離
    //[SerializeField, Tooltip("見える距離の設定")]
    //private float m_ViewingDistance;

    ////視野角
    //[SerializeField, Tooltip("視野角の設定")]
    //private float m_ViewingAngle;

    //プレイヤーの参照
    protected GameObject m_Player;

    ////プレイヤーへの注視点
    //Transform m_PlayerLookPoint;

    ////自身の目の位置
    //Transform m_EyePoint;

    // Use this for initialization
    public virtual void Start()
    {
        SetState();

        //m_Agent = GetComponent<NavMeshAgent>();

        ////目的地を設定する
        //SetNewPatrolPointToDestination();

        ////最初の状態を設定する
        //ChangeState(EnemyStatus.RoundState);

        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        //m_EyePoint = transform.Find("EyePoint");
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        m_Orders[m_OrderState](time, this);

        m_StateTimer += time;
    }


    // 命令の設定を行います
    protected virtual void SetState()
    {
        // 命令リストの取得
        m_StateList = this.transform.Find("StateList").GetComponent<EnemyStateList>();

        // 命令の追加
        for (int i = 0; i != m_StateList.GetEnemyStatus().Length; ++i)
        {
            var orders = m_StateList.GetEnemyState()[i];
            m_Orders.Add(m_StateList.GetEnemyStatus()[i], (deltaTime, gameObj) => { orders.Action(deltaTime, gameObj); });
        }
    }

    // 命令の変更を行います
    public virtual void ChangeState(EnemyStatus order)
    {
        // 命令がない場合は返す
        if (!CheckrState(order)) return;

        print("命令を変更しました");

        m_OrderState = order;
        m_StateTimer = 0.0f;
    }


    // 指定した状態があるかの確認を行います
    protected bool CheckrState(EnemyStatus order)
    {
        // 状態の追加
        for (int i = 0; i != m_StateList.GetEnemyStatus().Length; ++i)
        {
            var orderState = m_StateList.GetEnemyStatus()[i];
            // 同一の状態だった場合はtrueを返す
            if (order == orderState) return true;
        }
        // 同一の状態がない
        return false;
    }

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public virtual void onHear()
    {

    }

    public virtual void onDamage(int amount)
    {

    }


    //public bool CanSeePlayer()
    //{
    //    if (!IsPlayerInViewingDistance())
    //        return false;

    //    if (!IsPlayerInViewingAngle())
    //        return false;

    //    if (!CanHitRayToPlayer())
    //        return false;

    //    return true;
    //}

    //bool IsPlayerInViewingDistance()
    //{
    //    //自身からプレイヤーまでの距離
    //    float distanceToPlayer = Vector3.Distance(m_PlayerLookPoint.position, m_EyePoint.position);

    //    return (distanceToPlayer <= m_ViewingDistance);
    //}

    //bool IsPlayerInViewingAngle()
    //{
    //    //自身からプレイヤーへの方向ベクトル
    //    Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

    //    //自分の正面向きベクトルとプレイヤーへの方向ベクトルの差分角度
    //    float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

    //    //見える角度の範囲内にプレイヤーがいるかどうかを返却する
    //    return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    //}

    //bool CanHitRayToPlayer()
    //{
    //    //自身からプレイヤーへの方向ベクトル
    //    Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

    //    RaycastHit hitInfo;
    //    bool hit = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);

    //    //プレイヤーにRayが当たったかどうか返却する
    //    return (hit && hitInfo.collider.tag == "Player");

    //}


    //public bool HasArrived()
    //{
    //    return (Vector3.Distance(m_Agent.destination, transform.Find("FootPosition").position) < 0.5f);
    //}

    //public void SetNewPatrolPointToDestination()
    //{
    //    m_CurrentPatrolPointIndex = (m_CurrentPatrolPointIndex + 1) % m_RoundPoints.Length;

    //    m_Agent.destination = m_RoundPoints[m_CurrentPatrolPointIndex].position;
    //}

    //public void OnDrawGizmos()
    //{
    //    //視界の表示
    //    if (m_EyePoint != null)
    //    {
    //        //線の色
    //        Gizmos.color = new Color(0f, 0f, 1f);
    //        Vector3 eyePosition = m_EyePoint.position;
    //        Vector3 forward = m_EyePoint.forward * m_ViewingDistance;

    //        Gizmos.DrawRay(eyePosition, forward);
    //        Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, m_ViewingAngle, 0) * forward);
    //        Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, -m_ViewingAngle, 0) * forward);
    //    }

    //    //巡回ルートを描画
    //    if (m_RoundPoints != null)
    //    {
    //        Gizmos.color = new Color(0, 1, 0);

    //        for (int i = 0; i < m_RoundPoints.Length; i++)
    //        {
    //            int startIndex = i;
    //            int endIndex = i + 1;

    //            if (endIndex == m_RoundPoints.Length)
    //                endIndex = 0;

    //            Gizmos.DrawLine(m_RoundPoints[startIndex].position, m_RoundPoints[endIndex].position);
    //        }
}
