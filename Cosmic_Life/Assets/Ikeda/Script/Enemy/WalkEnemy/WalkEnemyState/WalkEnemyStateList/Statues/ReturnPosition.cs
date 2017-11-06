using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPosition : EnemyState {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("巡回しない戻る状態");

        WalkEnemy l_WalkEnemy = enemy.GetComponent<WalkEnemy>();

        //プレイヤーが見えている場合
        if (l_WalkEnemy.CanSeePlayer())
        {
            enemy.ChangeState(EnemyStatus.Chasing);
        }
        //見失った場合
        else
        {
            //元の位置に向かう
            l_WalkEnemy.m_Agent.destination = l_WalkEnemy.GetStartPosition();

            //元の位置と距離を比べる
            if (Vector3.Distance(l_WalkEnemy.GetStartPosition(), gameObject.transform.position) <= 0.2f)
            {
                l_WalkEnemy.AgentStop();

                //元の角度を向く
                l_WalkEnemy.transform.rotation = Quaternion.Slerp(l_WalkEnemy.transform.rotation, l_WalkEnemy.GetStartAngle(), Time.deltaTime * 2.0f);

                //元の向きに向き終わったか調べる
                if (Quaternion.Angle(l_WalkEnemy.transform.rotation, l_WalkEnemy.GetStartAngle()) <= 0.5f)
                {
                    //巡回しない状態に遷移
                    l_WalkEnemy.ChangeState(EnemyStatus.NonRoundState);
                }
            }
        }
    }
}
