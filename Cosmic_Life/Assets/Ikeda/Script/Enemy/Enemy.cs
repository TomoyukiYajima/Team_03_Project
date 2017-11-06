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


    //プレイヤーの参照
    protected GameObject m_Player;

    // Use this for initialization
    public virtual void Start()
    {
        SetState();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
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

    public virtual void IsDead()
    {
        if (m_Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
