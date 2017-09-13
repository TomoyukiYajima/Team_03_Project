using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    public enum EnemyState
    {
        //巡回
        RoundState,
        //追跡中
        Chasing,
        //追跡中(見失っている)
        ChasingButLosed
    }



    //[SerializeField, Tooltip("ターゲットを入れる")]
    //private Transform m_Target;

    //巡回のポイント
    [SerializeField, Tooltip("巡回のポイントを設定する")]
    private Transform[] m_RoundPoints;

    private NavMeshAgent m_Agent;

    //現在の巡回ポイントのインデックス
    private int m_CurrentPatrolPointIndex = -1;

    //見える距離
    [SerializeField, Tooltip("見える距離の設定")]
    private float m_ViewingDistance;

    //視野角
    [SerializeField, Tooltip("視野角の設定")]
    private float m_ViewingAngle;

    //プレイヤーの参照
    GameObject m_Player;

    //プレイヤーへの注視点
    Transform m_PlayerLookPoint;

    //自信の目の位置
    Transform m_EyePoint;

    //状態
    private EnemyState m_State = EnemyState.RoundState;

    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        //m_Agent.destination = m_Target.position;

        //目的地を設定する
        SetNewPatrolPointToDestination();

        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_State == EnemyState.RoundState)
        {
            print("巡回中");
            //プレイヤーが見えた場合
            if (CanSeePlayer())
            {
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
            }
            //プレイヤーが見えなくて、目的地に到着した場合
            else if (HasArrived())
            {
                //目的地を次の巡回ポイントに切り替える
                SetNewPatrolPointToDestination();
            }
        }
        //プレイヤーを追跡中
        else if (m_State == EnemyState.Chasing)
        {
            print("追跡中");
            //プレイヤーが見えている場合
            if (CanSeePlayer())
            {
                //プレイヤーの場所へ向かう
                m_Agent.destination = m_Player.transform.position;
            }
            //見失った場合
            else
            {
                //追跡中に状態変更
                m_State = EnemyState.ChasingButLosed;
            }
        }
        //追跡中の場合
        else if (m_State == EnemyState.ChasingButLosed)
        {
            print("見失い中");
            //プレイヤーが見えた場合
            if (CanSeePlayer())
            {
                //追跡中に状態変更
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
            }
            //プレイヤーを見つけられないまま目的地に到着
            else if (HasArrived())
            {
                //巡回中に状態遷移
                m_State = EnemyState.RoundState;
            }
        }

        //if (HasArrived())
        //{
        //    SetNewPatrolPointToDestination();
        //}

        //if (CanSeePlayer())
        //{
        //    print("見えている");
        //}
        //else
        //{
        //    print("見えていない");
        //}
    }

    void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex = (m_CurrentPatrolPointIndex + 1) % m_RoundPoints.Length;

        m_Agent.destination = m_RoundPoints[m_CurrentPatrolPointIndex].position;
    }

    private bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.Find("GameObject").position) < 0.5f);
    }

    bool IsPlayerInViewingDistance()
    {
        //自身からプレイヤーまでの距離
        float distanceToPlayer = Vector3.Distance(m_PlayerLookPoint.position, m_EyePoint.position);

        return (distanceToPlayer <= m_ViewingDistance);
    }

    bool IsPlayerInViewingAngle()
    {
        //自身からプレイヤーへの方向ベクトル
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        //自分の正面向きベクトルとプレイヤーへの方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        //見える角度の範囲内にプレイヤーがいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    bool CanHitRayToPlayer()
    {
        //自身からプレイヤーへの方向ベクトル
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);

        //プレイヤーにRayが当たったかどうか返却する
        return (hit && hitInfo.collider.tag == "Player");

    }

    bool CanSeePlayer()
    {
        if (!IsPlayerInViewingDistance())
            return false;

        if (!IsPlayerInViewingAngle())
            return false;

        if (!CanHitRayToPlayer())
            return false;

        return true;
    }



    public void OnDrawGizmos()
    {
        //視界の表示
        if (m_EyePoint != null)
        {
            //線の色
            Gizmos.color = new Color(0f, 0f, 1f);
            Vector3 eyePosition = m_EyePoint.position;
            Vector3 forward = m_EyePoint.forward * m_ViewingDistance;

            Gizmos.DrawRay(eyePosition, forward);
            Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, m_ViewingAngle, 0) * forward);
            Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, -m_ViewingAngle, 0) * forward);
        }

        //巡回ルートを描画
        if (m_RoundPoints != null)
        {
            Gizmos.color = new Color(0, 1, 0);

            for (int i = 0; i < m_RoundPoints.Length; i++)
            {
                int startIndex = i;
                int endIndex = i + 1;

                if (endIndex == m_RoundPoints.Length)
                    endIndex = 0;

                Gizmos.DrawLine(m_RoundPoints[startIndex].position, m_RoundPoints[endIndex].position);
            }
        }
    }
}