using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour {

    //状態列挙
    //[SerializeField]
    //private EnemyStatus m_OrderState = EnemyStatus.None;
    // 状態格納コンテナ
    //private Dictionary<EnemyStatus, System.Action<float, Enemy>> m_Orders =
    //    new Dictionary<EnemyStatus, System.Action<float, Enemy>>();

    //状態リスト
    //private EnemyStateList m_D_StateList;

    //状態実行時間
    protected float m_StateTimer = 0.0f;

    //巡回ルートの設定
    [SerializeField, Tooltip("巡回ルートを設定する")]
    private Transform[] m_RoutePositions;

    private int m_RoutePosCount = 0;

    [SerializeField, Tooltip("移動する速さの設定")]
    private float m_Speed = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //巡回Test
        Vector3 relativePos = m_RoutePositions[m_RoutePosCount].transform.position - this.transform.position;
        transform.Translate(relativePos.normalized * m_Speed * Time.deltaTime, Space.World);
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1.0f);
        if (Vector3.Distance(transform.position, m_RoutePositions[m_RoutePosCount].transform.position) <= 0.15f)
        {
            ArrivedProcessing();
        }
    }

    private void ArrivedProcessing()
    {
        if (m_RoutePosCount + 1 < m_RoutePositions.Length)
        {
            m_RoutePosCount++;
            /* 何も入ってなかったときに0に戻す */
            if (m_RoutePositions[m_RoutePosCount] == null) m_RoutePosCount = 0;
        }
        else
        {
            m_RoutePosCount = 0;
        }

    }
}
