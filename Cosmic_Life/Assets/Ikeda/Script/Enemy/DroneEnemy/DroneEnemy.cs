using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : Enemy
{

    [SerializeField, Tooltip("移動する速さの設定")]
    public float m_Speed = 0;

    [SerializeField, Tooltip("SearchLightを入れる")]
    private GameObject m_SearchLight;


    // Use this for initialization
    void Start()
    {
        base.Start();

        //最初の状態を設定する
        ChangeState(EnemyStatus.RoundState);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    float time = Time.deltaTime;
    //    m_States[m_DroneState](time, this);

    //    m_StateTimer += time;
    //}



    /// <summary>
    /// プレイヤーが見えたかどうか返す
    /// </summary>
    /// <returns></returns>
    public bool IsSeePlayer()
    {
        if (!SearchLightAngle()) return false;

        if (!CanHitRayToPlayer()) return false;

        return true;
    }


    /// <summary>
    /// プレイヤーが視野角内にいるか？
    /// </summary>
    private bool SearchLightAngle()
    {
        //スポットライトのSpotAngle
        float l_SpotAngle = m_SearchLight.GetComponent<Light>().spotAngle / 2;
        //自分からオブジェクトへの方向ベクトル(ワールド座標)
        Vector3 l_RelativeVec = m_Player.transform.position - m_SearchLight.transform.position;
        //自分の正面向きベクトルとオブジェクトへの方向ベクトルの差分角度
        float l_AngleToPlayer = Vector3.Angle(m_SearchLight.transform.forward, l_RelativeVec);
        //見える視野角の範囲内にオブジェクトがいるかどうかを返す
        return (Mathf.Abs(l_AngleToPlayer) <= l_SpotAngle);

        //Physics.Raycast(m_SearchLight.transform.position, l_RelativeVec);
    }

    /// <summary>
    /// SpotLightからRayを飛ばしてPlayerに当たるか？
    /// </summary>
    /// <returns></returns>
    private bool CanHitRayToPlayer()
    {
        //自分からPlayerへの方向ベクトル(ワールド座標)
        Vector3 l_RelativeVec = m_Player.transform.position - m_SearchLight.transform.position;
        //壁の向こう側にいる場合には見えない
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(m_SearchLight.transform.position, l_RelativeVec, out hitInfo);
        //オブジェクトにRayが当たったかどうかを返す
        return (hit && hitInfo.collider.tag == "Player");
    }

    public Transform[] GetRoundPoints()
    {
        return m_RoundPoints;
    }


    //void OnDrawGizmos()
    //{
    //    float l_SpotLightAngle = m_SearchLight.GetComponent<Light>().spotAngle / 2;
    //    //線の色
    //    Gizmos.color = new Color(0f, 1f, 0f);
    //    //目の位置
    //    Vector3 eyePosition = m_SearchLight.transform.position;
    //    //下向きの視線
    //    Vector3 bottom = transform.forward * l_SpotLightAngle;

    //    //下向きの視線を描画
    //    Gizmos.DrawRay(eyePosition, bottom);

    //    Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, 0, l_SpotLightAngle) * bottom);
    //    Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, 0, -l_SpotLightAngle) * bottom);
    //    Gizmos.DrawRay(eyePosition, Quaternion.Euler(l_SpotLightAngle, 0, 0) * bottom);
    //    Gizmos.DrawRay(eyePosition, Quaternion.Euler(-l_SpotLightAngle, 0, 0) * bottom);
    //    //Vector3 l_RelativeVec = m_Player.transform.position - m_SearchLight.transform.position;
    //    //Gizmos.DrawRay(m_SearchLight.transform.position, l_RelativeVec);
    //}


    public void ChangeColor()
    {
        if (IsSeePlayer())
            m_SearchLight.GetComponent<Light>().color = new Color(1, 0, 0, 1);

        else
            m_SearchLight.GetComponent<Light>().color = new Color(1, 1, 1, 1);
    }
}
