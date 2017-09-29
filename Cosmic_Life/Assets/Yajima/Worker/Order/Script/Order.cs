using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Order : MonoBehaviour {

    // 命令が終了したか
    protected bool m_IsEndOrder = false;
    // 格納された命令番号
    protected OrderNumber m_OrderNumber = OrderNumber.ONE;

    // Use this for initialization
    public virtual void Start() { }

    // Update is called once per frame
    public virtual void Update() { }

    // 最初の行動
    public virtual void StartAction(GameObject obj) { }

    // 行動
    public virtual void Action(float deltaTime, GameObject obj) { }

    // 行動終了
    public virtual void EndAction() { m_IsEndOrder = false; }

    // 停止時の行動
    public virtual void StopAction(GameObject obj)
    {
        m_IsEndOrder = true;
        ChangeOrder(obj, OrderStatus.STOP);
    }

    // 命令が終了したかを返します
    public bool IsEndOrder() { return m_IsEndOrder; }

    // 命令番号を設定します
    public void SetOrderNumber(OrderNumber number) { m_OrderNumber = number; }

    // 命令番号を取得します
    public OrderNumber GetOrderNumber() { return m_OrderNumber; }

    // 命令の変更
    protected void ChangeOrder(GameObject obj, OrderStatus status)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の変更)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.onOrder(status); });
    }

    // 命令の終了
    protected void EndOrder(GameObject obj)
    {
        // 相手側にイベントがなければ返す
        if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(obj)) return;
        // 実行(命令の終了)
        ExecuteEvents.Execute<IOrderEvent>(
            obj,
            null,
            (e, d) => { e.endOrder(m_OrderNumber); });
    }
}
