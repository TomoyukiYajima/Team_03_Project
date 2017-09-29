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

    public override void Action(float deltaTime, Enemy obj)
    {
        print("追跡中(見失い中)");

        //プレイヤーが見えた場合
        if (obj.GetComponent<WalkEnemy>().CanSeePlayer())
        {
            //追跡中に状態変更
            obj.ChangeState(EnemyStatus.Chasing);
            obj.GetComponent<WalkEnemy>().m_Agent.destination = obj.GetPlayer().transform.position;
        }
        //プレイヤーを見つけられないまま目的地に到着
        else if (obj.GetComponent<WalkEnemy>().HasArrived())
        {
            //巡回中に状態遷移
            obj.ChangeState(EnemyStatus.RoundState);
        }

    }

}
