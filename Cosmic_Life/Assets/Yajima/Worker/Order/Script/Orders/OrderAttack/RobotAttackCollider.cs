using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotAttackCollider : MonoBehaviour
{
    [SerializeField]
    private int m_Damage = 1;   // ダメージ量

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") return;

        // onDamage を呼ぶ
        // 相手側にイベントがなければ返す
        //if (!ExecuteEvents.CanHandleEvent<IEnemyEvent>(other.gameObject)) return;
        //// 実行(ダメージ処理)
        //ExecuteEvents.Execute<IEnemyEvent>(
        //    other.gameObject,
        //    null,
        //    (e, d) => { e.onDamage(m_Damage); });

        if (!ExecuteEvents.CanHandleEvent<IGeneralEvent>(other.gameObject)) return;
        // 実行(ダメージ処理)
        ExecuteEvents.Execute<IGeneralEvent>(
            other.gameObject,
            null,
            (e, d) => { e.onDamage(m_Damage); });
    }
}
