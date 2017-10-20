using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLiftObject : MonoBehaviour
{

    // 衝突したオブジェクト
    private GameObject m_LiftObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 持ち上げるオブジェクトに辿り着いたかを返します
    public bool IsCheckLift(GameObject obj)
    {
        if (m_LiftObject != obj) return false;
        // 到達した
        return true;
    }

    // 登録したオブジェクトを解放します
    public void ReleaseObject()
    {
        m_LiftObject = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag != "StageObject") return;
        // 衝突したオブジェクトを入れる
        m_LiftObject = other.gameObject;
    }
}
