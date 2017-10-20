using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyAttack : EnemyState {

    private enum AttackState
    {
        Attack,
        AttackAfter,

        None
    }

    //攻撃後の距離比較
    [SerializeField]
    private float m_DistanceCompare = 1.0f;

    //衝突判定オブジェクト
    [SerializeField]
    private GameObject m_AttackCollider;

    [SerializeField]
    private float m_AttackTime = 15.0f; 

    private AttackState m_AttackState;

    private float m_Timer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}

    //void Update()
    //{

    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        switch(m_AttackState)
        {
            case AttackState.Attack:
                m_AttackCollider.SetActive(true);
                if (m_AttackTime > m_Timer) m_Timer +=  1.0f * Time.deltaTime;
                else
                {
                    m_Timer = 0;
                    m_AttackState = AttackState.AttackAfter;
                }
                break;

            case AttackState.AttackAfter:
                m_AttackCollider.SetActive(false);
                Vector3 l_PlayerPosition = enemy.GetPlayer().transform.position;

                //攻撃後のプレイヤーとの距離を測って、離れていたら追跡中に変更
                if (Vector3.Distance(transform.position, l_PlayerPosition) <= m_DistanceCompare) m_AttackState = AttackState.Attack;
                else
                {
                    enemy.ChangeState(EnemyStatus.Chasing);
                    m_AttackState = AttackState.Attack;
                }
                break;
        }
    }
}
