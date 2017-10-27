using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IGimmickEvent : IEventSystemHandler
{
    /// <summary>
    /// 起動するイベント
    /// </summary>
    void onActivate(GameObject obj);

    void onActivate(string password);
    void onActivate();
}
