using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : EnemyState {

    //攻撃を当てる距離の設定
    [SerializeField]
    private float m_StopDistance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("追跡中");
        WalkEnemy l_WalkEnemy = enemy.GetComponent<WalkEnemy>();

        //プレイヤーが見えている場合
        if (l_WalkEnemy.CanSeePlayer())
        {
            //プレイヤーの場所へ向かう
            l_WalkEnemy.m_Agent.destination = l_WalkEnemy.GetPlayer().transform.position;
            //プレイヤーとの距離
            float distance = Vector3.Distance(l_WalkEnemy.GetEnemyPosition(), l_WalkEnemy.GetPlayer().transform.position);

            //プレイヤーとの距離を調べて、近ければ攻撃状態へ
            if (distance < m_StopDistance)
            {
                l_WalkEnemy.AgentStop();
                enemy.ChangeState(EnemyStatus.Attack);
            }
        }
        //見失った場合
        else
        {
            //追跡中(見失い)に状態変更
            enemy.ChangeState(EnemyStatus.ChasingButLosed);
        }
    }
}
