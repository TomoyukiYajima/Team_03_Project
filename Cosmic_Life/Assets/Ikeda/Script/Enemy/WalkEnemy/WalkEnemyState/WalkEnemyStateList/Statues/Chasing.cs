using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : EnemyState {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("追跡中");

        //プレイヤーが見えている場合
        if (enemy.GetComponent<WalkEnemy>().CanSeePlayer())
        {
            //プレイヤーの場所へ向かう
            enemy.GetComponent<WalkEnemy>().m_Agent.destination = enemy.GetPlayer().transform.position - transform.forward * 1.3f;

            float distance = Vector3.Distance(transform.position, enemy.GetPlayer().transform.position);

            if (distance <= 1.5f)
            {
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
