using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IGeneralEvent : IEventSystemHandler {
    /// <summary>
    /// 攻撃を受けたときのイベント
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    void onDamage(int amount);

    //void onShock();

    //void onThrow();

    //void onLift(GameObject obj);

    //void onTakeDown();
}
