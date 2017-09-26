﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IOrderEvent : IEventSystemHandler {
    // 命令の変更
    void onOrder(OrderStatus order);
    // 命令の変更(方向指定)
    void onOrder(OrderStatus order, OrderDirection direction);
    // 命令の終了
    void endOrder();
}
