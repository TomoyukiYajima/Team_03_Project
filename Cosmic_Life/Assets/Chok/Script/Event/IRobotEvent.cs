using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum OrderDirection
{
    NULL        = 1 << 0,
    FORWARD     = 1 << 1,
    BACKWARD    = 1 << 2,
    UP          = 1 << 3,
    DOWN        = 1 << 4,
    LEFT        = 1 << 5,
    RIGHT       = 1 << 6,
}

public interface IRobotEvent : IEventSystemHandler,IGeneralEvent {
    //void onOrder(OrderStatus order);
    //void onOrder(OrderStatus order, OrderDirection direction);
}
