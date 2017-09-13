using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IWorkerEvent : IEventSystemHandler {
    void onOrder(OrderStatus order);
}
