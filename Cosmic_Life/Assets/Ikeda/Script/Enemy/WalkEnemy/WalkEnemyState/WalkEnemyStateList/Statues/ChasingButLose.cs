using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingButLose : EnemyState {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("追跡中(見失い中)");

        //プレイヤーが見えた場合
        if (enemy.GetComponent<WalkEnemy>().CanSeePlayer())
        {
            enemy.GetComponent<WalkEnemy>().m_Agent.destination = enemy.GetPlayer().transform.position;
            //追跡中に状態変更
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //プレイヤーを見つけられないまま目的地に到着
        else if (enemy.GetComponent<WalkEnemy>().HasArrived())
        {
            if (enemy.GetComponent<WalkEnemy>().GetIsPatrol())
            {
                //巡回中に状態遷移
                enemy.ChangeState(EnemyStatus.RoundState);

            }
            else {
                //元の位置に戻る状態
                enemy.ChangeState(EnemyStatus.ReturnPosition);

            }
        }

    }
}
