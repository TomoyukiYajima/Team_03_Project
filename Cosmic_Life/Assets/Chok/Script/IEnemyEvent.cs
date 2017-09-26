using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IEnemyEvent : IEventSystemHandler
{
    void onHear();
    void onDamage(int amount);
}
