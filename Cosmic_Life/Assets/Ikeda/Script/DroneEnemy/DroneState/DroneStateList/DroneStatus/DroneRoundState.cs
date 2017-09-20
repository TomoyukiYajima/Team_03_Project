using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRoundState : DroneState {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public override void Action(float deltaTime, DroneEnemy dEnemy)
    {
        Vector3 relativePos = dEnemy.m_RoutePositions[dEnemy.m_RoutePosCount].transform.position - dEnemy.transform.position;
        dEnemy.transform.Translate(relativePos.normalized * dEnemy.m_Speed * Time.deltaTime, Space.World);
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        dEnemy.transform.rotation = Quaternion.Slerp(dEnemy.transform.rotation, rotation, Time.deltaTime * 1.0f);
        if (Vector3.Distance(dEnemy.transform.position, dEnemy.m_RoutePositions[dEnemy.m_RoutePosCount].transform.position) <= 0.15f)
        {
            ArrivedProcessing(dEnemy);
        }

            dEnemy.ChangeColor();
    }

    private void ArrivedProcessing(DroneEnemy dEnemy)
    {
        if (dEnemy.m_RoutePosCount + 1 < dEnemy.m_RoutePositions.Length)
        {
            dEnemy.m_RoutePosCount++;
            /* 何も入ってなかったときに0に戻す */
            if (dEnemy.m_RoutePositions[dEnemy.m_RoutePosCount] == null) dEnemy.m_RoutePosCount = 0;
        }
        else
        {
            dEnemy.m_RoutePosCount = 0;
        }
    }
}
