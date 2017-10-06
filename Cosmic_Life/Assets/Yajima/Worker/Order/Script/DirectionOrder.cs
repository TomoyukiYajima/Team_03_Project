using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOrder : Order {

    protected OrderDirection m_OrderDirection;

    // Use this for initialization
    public override void Start()
    {

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void StartAction(GameObject obj)
    {
        base.StartAction(obj);
        // 方向の設定
        SetDirection(obj);
    }

    public virtual void StartAction(GameObject obj, OrderDirection dir)
    {
        StartAction(obj);
        // 命令方向の変更
        m_OrderDirection = dir;
        // 方向の設定
        SetDirection(obj);
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        base.Action(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);
    }

    // 移動方向の設定
    protected virtual void SetDirection(GameObject obj)
    {

    }

    // 命令方向の取得
    public OrderDirection GetDirection() { return m_OrderDirection; }
}
