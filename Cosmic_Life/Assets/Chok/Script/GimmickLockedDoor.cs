using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimmickLockedDoor : GimmickBase
{

    [SerializeField, Tooltip("ロックを解除するパスワード")] private string m_password;
    //[SerializeField, Tooltip("受付可能の距離")] private float m_distance = 10.0f;
    [SerializeField] private GameObject m_doorLeft;
    [SerializeField] private GameObject m_doorRight;
    [SerializeField] private GameObject m_lock;
    [SerializeField] private GameObject m_leftPoint;
    [SerializeField] private GameObject m_rightPoint;

    private bool m_isReceptable = false; // 受付可能か

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) onActivate("ひらけごま");
    }

    public override void onActivate(string password)
    {
        // すでに起動されたならreturn
        if (m_isActivated) return;
        // 受付できない状態ならreturn
        if (!m_isReceptable) return;

        //// プレイヤーあるいはロボットとの距離が遠いければreturn
        //float distanceP = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        //float distanceR = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Robot").transform.position);

        //if (distanceP > m_distance && distanceR > m_distance) return;

        // パスワードが正しいか
        bool isValid = false;
        // 入力したパスワードがロックのパスワードが同じならtrue
        if (password == m_password) isValid = true;
        // パスワードの真偽によって処理を分ける
        IEnumerator coroutine = isValid == true ? ValidPassword() : InvalidPassword();
        // 処理をはじめる
        StartCoroutine(coroutine);
        m_isActivated = true;
    }

    private IEnumerator InvalidPassword()
    {
        yield return null;
    }
    private IEnumerator ValidPassword()
    {
        m_lock.transform.DORotateQuaternion(m_leftPoint.transform.rotation, 2.0f);
        yield return new WaitForSeconds(3.0f);

        m_doorLeft.transform.DOMove(m_leftPoint.transform.position, 2.0f);
        m_doorRight.transform.DOMove(m_rightPoint.transform.position, 2.0f);

        yield return new WaitForSeconds(2.0f);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        m_isReceptable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        m_isReceptable = false;
    }


}
