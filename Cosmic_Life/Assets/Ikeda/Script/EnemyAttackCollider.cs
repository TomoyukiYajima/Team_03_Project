using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyAttackCollider : MonoBehaviour
{
    private int m_Damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!ExecuteEvents.CanHandleEvent<IEnemyEvent>(other.gameObject)) return;
        //実行
        ExecuteEvents.Execute<IEnemyEvent>(other.gameObject, null, (e, d) => { e.onDamage(m_Damage); });
    }
}
