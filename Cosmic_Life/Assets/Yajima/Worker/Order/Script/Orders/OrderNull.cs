using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 命令なしクラス
public class OrderNull : Order {

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Null");
    }
}
