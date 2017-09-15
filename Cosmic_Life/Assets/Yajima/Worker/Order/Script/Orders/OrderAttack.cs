using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 攻撃命令クラス
public class OrderAttack : Order {

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Attack");
    }
}
