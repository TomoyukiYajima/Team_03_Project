using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum OrderDirection
{
    FORWARD     = 1 << 0,
    BACKWARD    = 1 << 1,
    UP          = 1 << 2,
    DOWN        = 1 << 3,
    LEFT        = 1 << 4,
    RIGHT       = 1 << 5,
}

public interface IRobotEvent : IEventSystemHandler {
    void onOrder(OrderStatus order,OrderDirection direction);
}
