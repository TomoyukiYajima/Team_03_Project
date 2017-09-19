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

    public override void Action(float deltaTime, Enemy obj)
    {
        print("追跡中");

        //プレイヤーが見えている場合
        if (obj.CanSeePlayer())
        {
            //プレイヤーの場所へ向かう
            obj.m_Agent.destination = obj.m_Player.transform.position;
        }
        //見失った場合
        else
        {
            //追跡中(見失い)に状態変更
            obj.ChangeState(EnemyStatus.ChasingButLosed);
        }
    }
}
