using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrderHighAttack : OrderAttack {

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        m_OrderText = "HighAttack";
    }

    //// Update is called once per frame
    //void Update () {

    //}

    //public override void Action(float deltaTime, GameObject obj)
    //{
    //    print("HighAttack");

    //    // Tweenの移動
    //    //this.transform.DOMove(m_Collider.transform.position, 1.0f);

    //    ////
    //    //var length = Vector3.Distance(m_StopPoint.position, m_Collider.transform.position);
    //    //if (length <= 0.1f) return;
    //    //var dir = (m_StopPoint.position - m_Collider.transform.position).normalized;
    //    //m_Collider.transform.position += dir * 1.0f * Time.deltaTime;
    //}
}
