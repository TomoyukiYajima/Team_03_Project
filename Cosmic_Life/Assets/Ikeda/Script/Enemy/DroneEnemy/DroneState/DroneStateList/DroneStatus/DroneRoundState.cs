using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRoundState : EnemyState
{

    private int m_RoutePosCount = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void Action(float deltaTime, Enemy enemy)
    {
        Vector3 relativePos = enemy.GetComponent<DroneEnemy>().GetRoundPoints()[m_RoutePosCount].transform.position - enemy.GetComponent<DroneEnemy>().transform.position;
        enemy.GetComponent<DroneEnemy>().transform.Translate(relativePos.normalized * enemy.GetComponent<DroneEnemy>().m_Speed * Time.deltaTime, Space.World);
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        enemy.GetComponent<DroneEnemy>().transform.rotation = Quaternion.Slerp(enemy.GetComponent<DroneEnemy>().transform.rotation, rotation, Time.deltaTime * 1.0f);
        if (Vector3.Distance(enemy.GetComponent<DroneEnemy>().transform.position, enemy.GetComponent<DroneEnemy>().GetRoundPoints()[m_RoutePosCount].transform.position) <= 0.15f)
        {
            ArrivedProcessing(enemy);
        }

        enemy.GetComponent<DroneEnemy>().ChangeColor();
    }

    private void ArrivedProcessing(Enemy enemy)
    {
        if (m_RoutePosCount + 1 < enemy.GetComponent<DroneEnemy>().GetRoundPoints().Length)
        {
            m_RoutePosCount++;
            /* 何も入ってなかったときに0に戻す */
            if (enemy.GetComponent<DroneEnemy>().GetRoundPoints()[m_RoutePosCount] == null) m_RoutePosCount = 0;
        }
        else
        {
            m_RoutePosCount = 0;
        }
    }
}
