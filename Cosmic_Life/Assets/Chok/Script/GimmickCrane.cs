using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GimmickCrane : GimmickBase
{
    [SerializeField] private float m_activateDuration;
    [SerializeField] private float m_waitDuration;
    [SerializeField] private Transform[] m_points;
    [SerializeField] private Transform m_dropPoint;
    [SerializeField] private GameObject m_holdPoint;

    private int m_point;
    private GameObject m_holdObj;
    private bool m_holding;

    // Use this for initialization
    void Start()
    {
        m_point = 0;
        m_holding = false;
    }

    private void Update()
    {
        if (m_holding)
        {
            m_holdObj.transform.position = m_holdPoint.transform.position;
        }
    }

    private IEnumerator HoldObject()
    {
        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        if (m_holdObj.tag == "Robot")
        {
            transform.DOMove(m_dropPoint.position, m_activateDuration);
        }
        else
        {
            m_point = (m_point + 1) % m_points.Length;
            transform.DOMove(m_points[m_point].position, m_activateDuration);
        }

        yield return new WaitForSeconds(m_activateDuration);

        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        ReleaseObject();

        ResetCrane();

        yield return null;
    }

    private IEnumerator HoldRobot()
    {
        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        transform.DOMove(m_dropPoint.position, m_activateDuration);

        yield return new WaitForSeconds(m_activateDuration);

        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        // ロボットを落とす

        yield return new WaitForSeconds(m_waitDuration);

        yield return null;
    }

    private IEnumerator ResetCrane()
    {
        if(Vector3.Distance(transform.position, m_points[m_point].position) > 1.0f)
        {
            // 戻る
            transform.DOMove(m_points[m_point].position, m_activateDuration);

            yield return new WaitForSeconds(m_activateDuration);

            // SEやEffectを出す
            yield return new WaitForSeconds(m_waitDuration);
        }

        m_isActivated = false;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Robot") return;

        if (m_isActivated) return;

        m_holdObj = other.gameObject;

        onActivate(m_holdObj);

        //if (other.tag == "Player")
        //{
        //    var player = m_holdObj.GetComponent<Player>();
        //    player.HoldCrane(m_holdPoint);
        //    StartCoroutine(HoldObject());
        //    Debug.Log("HoldPlayer");
        //}
        //else
        //{
        //    // IRobotEventが実装されていなければreturn
        //    if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(m_holdObj))
        //    {
        //        Debug.Log("IOrderEvent未実装");
        //        return;
        //    }

        //    ExecuteEvents.Execute<IOrderEvent>(
        //        m_holdObj,
        //        null,
        //        (receive, y) => receive.onOrder(OrderStatus.NULL));
        //    StartCoroutine(HoldRobot());
        //    Debug.Log("HoldRobot");
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Robot") return;
        m_holdObj = null;
        StopAllCoroutines();
        StartCoroutine(ResetCrane());
    }

    public override void onActivate(GameObject obj)
    {
        if (m_isActivated) return;
        m_isActivated = true;

        Debug.Log("Activate");
        // 持ち上げるオブジェクトを格納
        m_holdObj = obj;

        // IGeneralEventが実装されていなければreturn
        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(m_holdObj))
        {
            Debug.Log(m_holdObj + "はIGeneralEvent未実装");
            return;
        }

        Debug.Log("イベント開始IGeneralEvent/onLift, Object = " + m_holdObj.gameObject);
        //ExecuteEvents.Execute<IGimmickEvent>(
        //    m_holdObj,
        //    null,
        //    (receive, y) => receive.onLift());


        StartCoroutine(HoldObject());
    }

    private void ReleaseObject()
    {
        // IGeneralEventが実装されていなければreturn
        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(m_holdObj))
        {
            Debug.Log(m_holdObj + "はIGeneralEvent未実装");
            return;
        }

        Debug.Log("イベント開始IGeneralEvent/onTakeDown, Object = " + m_holdObj.gameObject);
        //ExecuteEvents.Execute<IGimmickEvent>(
        //    m_holdObj,
        //    null,
        //    (receive, y) => receive.onTakeDown());
    }
}
