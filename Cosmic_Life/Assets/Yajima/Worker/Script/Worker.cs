using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour {

    // 命令
    protected OrderStatus m_OrderState = OrderStatus.NULL;
    // 命令格納コンテナ
    private Dictionary<OrderStatus, Action<float, GameObject>> m_Orders =
        new Dictionary<OrderStatus, Action<float, GameObject>>();
    // 命令リスト
    protected OrderList m_OrderList;
    // 時間
    protected float m_StateTimer = 0.0f;

    // Use this for initialization
    public virtual void Start () {
        // 命令の設定
        SetOrder();
    }

    // Update is called once per frame
    public virtual void Update () {
        // デルタタイムの取得
        float time = Time.deltaTime;
        // 命令の実行
        m_Orders[m_OrderState](time, gameObject);

        // 命令(仮)　音声認識でプレイヤーから命令してもらう
        // OKボタンが押されたら、移動命令を行う
        if (PlayerInputManager.GetInputDown(InputState.INPUT_OK)) ChangeOrder(OrderStatus.MOVE);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_CANCEL)) ChangeOrder(OrderStatus.STOP);

        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) ChangeOrder(OrderStatus.TURN_LEFT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) ChangeOrder(OrderStatus.TURN_RIGHT);

        m_StateTimer += time;
    }

    // 命令の設定を行います
    protected virtual void SetOrder()
    {
        // 命令リストの取得
        m_OrderList = this.transform.Find("OrderList").GetComponent<OrderList>();

        // 命令の追加
        for(int i = 0; i != m_OrderList.GetOrderStatus().Length; ++i)
        {
            var orders = m_OrderList.GetOrders()[i];
            m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime, gameObj) => { orders.Action(deltaTime, gameObj); });
            // 下記だと、iの値が変に加算されてしまうので注意！
            // m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime) => { m_OrderList.GetOrders()[i].Action(); });
        }
    }

    // 命令の変更を行います
    public virtual void ChangeOrder(OrderStatus order)
    {
        m_OrderState = order;
        m_StateTimer = 0.0f;
    }
}
