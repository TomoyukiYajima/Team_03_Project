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

    public override void Action(float deltaTime, Enemy obj)
    {
        print("巡回中");

        //プレイヤーが見えた場合
        if (obj.CanSeePlayer())
        {
            obj.ChangeState(EnemyStatus.Chasing);
            obj.m_Agent.destination = obj.m_Player.transform.position;
        }
        //プレイヤーが見えなくて、目的地に到着した場合
        else if (obj.HasArrived())
        {
            //目的地を次の巡回ポイントに切り替える
            obj.SetNewPatrolPointToDestination();
        }
    }
}
