using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Action(float deltaTime, Enemy enemy)
    {
        print("巡回中");

        //プレイヤーが見えた場合
        if (enemy.GetComponent<WalkEnemy>().CanSeePlayer())
        {
            enemy.ChangeState(EnemyStatus.Chasing); 
            //obj.GetComponent<WalkEnemy>().m_Agent.destination = obj.GetPlayer().transform.position;
        }
        //プレイヤーが見えなくて、目的地に到着した場合
        else if (enemy.GetComponent<WalkEnemy>().HasArrived())
        {
            //目的地を次の巡回ポイントに切り替える
            enemy.GetComponent<WalkEnemy>().SetNewPatrolPointToDestination();
        }
    }
}
