using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 命令なしクラス
public class OrderNull : Order {

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Null");
    }
}
