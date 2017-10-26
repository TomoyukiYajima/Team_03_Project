using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトを持っている
public class OrderLiftMove : MonoBehaviour {

    // 持ち上げているオブジェクト
    private GameObject m_LiftObject;
    // 持ち上げたオブジェクトの初期角度
    private Vector3 m_InitAngle;

    //   // Use this for initialization
    //   void Start () {

    //}

    //   // Update is called once per frame
    //   void Update()
    //   {

    //   }

    public void CheckLiftObject(GameObject obj)
    {
        //base.StartAction(obj);

        //m_Timer = 0.0f;
        //m_IsEndOrder = false;

        // 持ち上げているオブジェクトを確かめる
        var liftObj = obj.transform.Find("LiftObject");
        // もし何も持っていなければ、返す
        if (liftObj.childCount == 0)
        {
            print("何も持っていません");
            // 攻撃状態に遷移
            //ChangeOrder(obj, OrderStatus.ATTACK);
            return;
        }

        // 違うオブジェクトの場合
        if (m_LiftObject != liftObj.GetChild(0).gameObject)
        {
            m_LiftObject = liftObj.GetChild(0).gameObject; //liftObj.GetChild(0).GetComponent<StageObject>();
            //m_LiftObject.transform.position = m_StartPoint.position;
            //m_MoveObject = m_LiftObject;
            m_InitAngle = m_LiftObject.transform.eulerAngles;
            // 持ち上げているオブジェクトの衝突判定を設定する
            GameObject collider = m_LiftObject.transform.Find("Collider").gameObject;
            //if (collider != null) m_Collider = collider;
        }

        //// Tweenの移動
        //m_LiftObject.transform.DOMove(m_StopPoint.position, m_AttackTime);

        // 持ち上げているオブジェクトの衝突判定をオンにする
        //m_Collider.SetActive(true);
    }

    // 持ち上げているオブジェクトの取得
    public GameObject GetLiftObject() { return m_LiftObject; }
}
