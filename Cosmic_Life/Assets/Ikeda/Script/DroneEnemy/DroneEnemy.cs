using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{

    //状態列挙
    [SerializeField]
    private DroneEnemyStatus m_DroneState = DroneEnemyStatus.None;

    // 状態格納コンテナ
    private Dictionary<DroneEnemyStatus, System.Action<float, DroneEnemy>> m_States =
        new Dictionary<DroneEnemyStatus, System.Action<float, DroneEnemy>>();

    //状態リスト
    private DroneStateList m_D_StateList;

    //状態実行時間
    protected float m_StateTimer = 0.0f;

    //巡回ルートの設定
    [SerializeField, Tooltip("巡回ルートを設定する")]
    public Transform[] m_RoutePositions;

    [System.NonSerialized]
    public int m_RoutePosCount = 0;

    [SerializeField, Tooltip("移動する速さの設定")]
    public float m_Speed = 0;

    [SerializeField, Tooltip("SearchLightを入れる")]
    private GameObject m_SearchLight;

    [SerializeField]
    private GameObject m_Player;

    // Use this for initialization
    void Start()
    {
        SetState();

        //最初の状態を設定する
        ChangeState(DroneEnemyStatus.RoundState);
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        m_States[m_DroneState](time, this);

        m_StateTimer += time;
    }

    // 命令の設定を行います
    protected virtual void SetState()
    {
        // 命令リストの取得
        m_D_StateList = this.transform.Find("DroneStateList").GetComponent<DroneStateList>();

        // 命令の追加
        for (int i = 0; i != m_D_StateList.GetDroneStatus().Length; ++i)
        {
            var states = m_D_StateList.GetDroneState()[i];
            m_States.Add(m_D_StateList.GetDroneStatus()[i], (deltaTime, gameObj) => { states.Action(deltaTime, gameObj); });
        }
    }

    // 命令の変更を行います
    public virtual void ChangeState(DroneEnemyStatus state)
    {
        // 命令がない場合は返す
        if (!CheckrState(state)) return;

        print("命令を変更しました");

        m_DroneState = state;
        m_StateTimer = 0.0f;
    }


    // 指定した状態があるかの確認を行います
    protected bool CheckrState(DroneEnemyStatus state)
    {
        // 状態の追加
        for (int i = 0; i != m_D_StateList.GetDroneStatus().Length; ++i)
        {
            var orderState = m_D_StateList.GetDroneStatus()[i];
            // 同一の状態だった場合はtrueを返す
            if (state == orderState) return true;
        }
        // 同一の状態がない
        return false;
    }


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


    void OnDrawGizmos()
    {
        float l_SpotLightAngle = m_SearchLight.GetComponent<Light>().spotAngle / 2;
        //線の色
        Gizmos.color = new Color(0f, 1f, 0f);
        //目の位置
        Vector3 eyePosition = m_SearchLight.transform.position;
        //下向きの視線
        Vector3 bottom = transform.forward * l_SpotLightAngle;

        //下向きの視線を描画
        Gizmos.DrawRay(eyePosition, bottom);

        Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, 0, l_SpotLightAngle) * bottom);
        Gizmos.DrawRay(eyePosition, Quaternion.Euler(0, 0, -l_SpotLightAngle) * bottom);
        Gizmos.DrawRay(eyePosition, Quaternion.Euler(l_SpotLightAngle, 0, 0) * bottom);
        Gizmos.DrawRay(eyePosition, Quaternion.Euler(-l_SpotLightAngle, 0, 0) * bottom);
        Vector3 l_RelativeVec = m_Player.transform.position - m_SearchLight.transform.position;
        Gizmos.DrawRay(m_SearchLight.transform.position, l_RelativeVec);
    }


    public void ChangeColor()
    {
        if (IsSeePlayer())
            m_SearchLight.GetComponent<Light>().color = new Color(1, 0, 0, 1);

        else
            m_SearchLight.GetComponent<Light>().color = new Color(1, 1, 1, 1);

    }
}
