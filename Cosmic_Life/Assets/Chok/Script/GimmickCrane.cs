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

    // Use this for initialization
    void Start()
    {
        m_point = 0;
    }

    private IEnumerator HoldPlayer()
    {
        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        m_point = (m_point + 1) % m_points.Length;
        transform.DOMove(m_points[m_point].position, m_activateDuration);

        yield return new WaitForSeconds(m_activateDuration);

        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        var player = m_holdObj.GetComponent<Player>();
        player.EndState();

        m_isActivated = false;

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

        // 戻る
        transform.DOMove(m_points[m_point].position, m_activateDuration);

        yield return new WaitForSeconds(m_activateDuration);

        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        m_isActivated = false;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Robot") return;

        if (m_isActivated) return;
        m_isActivated = true;

        m_holdObj = other.gameObject;
        Debug.Log("Activate");

        if (other.tag == "Player")
        {
            var player = m_holdObj.GetComponent<Player>();
            player.HoldCrane(m_holdPoint);
            StartCoroutine(HoldPlayer());
            Debug.Log("HoldPlayer");
        }
        else
        {
            // IRobotEventが実装されていなければreturn
            if (!ExecuteEvents.CanHandleEvent<IOrderEvent>(m_holdObj))
            {
                Debug.Log("IOrderEvent未実装");
                return;
            }

            ExecuteEvents.Execute<IOrderEvent>(
                m_holdObj,
                null,
                (receive, y) => receive.onOrder(OrderStatus.NULL));
            StartCoroutine(HoldRobot());
            Debug.Log("HoldRobot");
        }



    }


}
