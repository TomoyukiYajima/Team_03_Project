using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 薙ぎ払い命令
public class OrderMowDown : OrderAttack {

    // 持ち上げているオブジェクト
    private GameObject m_LiftObject;
    // 持ち上げたオブジェクトの初期角度
    private Vector3 m_InitAngle;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        //base.StartAction(obj);

        m_Timer = 0.0f;
        m_IsEndOrder = false;

        // 持ち上げているオブジェクトを確かめる
        var liftObj = obj.transform.Find("LiftObject");
        // もし何も持っていなければ、返す
        if (liftObj.childCount == 0)
        {
            print("何も持っていません");
            // 攻撃状態に遷移
            ChangeOrder(obj, OrderStatus.ATTACK);
            return;
        }

        // 違うオブジェクトの場合
        if(m_LiftObject != liftObj.GetChild(0).gameObject)
        {
            m_LiftObject = liftObj.GetChild(0).gameObject; //liftObj.GetChild(0).GetComponent<StageObject>();
            m_LiftObject.transform.position = m_StartPoint.position;
            m_MoveObject = m_LiftObject;
            m_InitAngle = m_LiftObject.transform.eulerAngles;
            // 持ち上げているオブジェクトの衝突判定を設定する
            GameObject collider = m_LiftObject.transform.Find("Collider").gameObject;
            if (collider != null) m_Collider = collider;
        }
        // Tweenの移動
        m_LiftObject.transform.DOMove(m_StopPoint.position, m_AttackTime);
        // 持ち上げているオブジェクトの衝突判定をオンにする
        m_Collider.SetActive(true);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        // 持ち上げているオブジェクトの角度を調整する
        //Vector2 forward = new Vector2(obj.transform.forward.x, obj.transform.forward.z);
        Vector3 vec = m_LiftObject.transform.position - obj.transform.position;
        float angleX = Vector2.Angle(
            new Vector2(obj.transform.forward.x, obj.transform.forward.y),
            new Vector2(vec.x, vec.y));
        //float angleY = Vector2.Angle(
        //    new Vector2(obj.transform.forward.x, obj.transform.forward.z),
        //    new Vector2(vec.x, vec.z));
        //if (angleY < 0.0f) angleY = 360 -angleY;
        //angleY += 180;
        //if (angleY < 0.0f) angleY *= -1;
        // 相手の方向を向く(仮)
        float angleY = Mathf.Atan2(vec.z, vec.x) * Mathf.Rad2Deg;

        //m_LiftObject.transform.LookAt(angleX, angleY, m_LiftObject.transform.eulerAngles.z);

        m_LiftObject.transform.eulerAngles = new Vector3(m_LiftObject.transform.eulerAngles.x, m_InitAngle.y - angleY + (90+ obj.transform.eulerAngles.y), m_LiftObject.transform.eulerAngles.z);



        base.UpdateAction(deltaTime, obj);
    }

    public override void EndAction(GameObject obj)
    {
        //base.EndAction(obj);
        if (m_LiftObject != null) m_LiftObject.transform.position = m_StartPoint.position;
    }

    protected override void MoveObject()
    {
        // // Tweenの移動
        m_LiftObject.transform.DOMove(m_StartPoint.position, m_BackTime / 2);
    }
}
