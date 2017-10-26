using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 守備命令クラス
public class OrderProtect : Order {

    // バリアオブジェクト
    private GameObject m_Barrier;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if(m_Barrier == null) m_Barrier = this.transform.Find("Barrier").gameObject;
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        // バリアの表示
        if (m_Barrier.activeSelf) return;
        m_Barrier.gameObject.SetActive(true);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Protect");
    }

    public override void EndAction(GameObject obj)
    {
        base.EndAction(obj);
        // バリアの非表示
        m_Barrier.gameObject.SetActive(false);
    }
}
