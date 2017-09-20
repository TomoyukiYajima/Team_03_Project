using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStateList : MonoBehaviour {

    [SerializeField]
    private DroneEnemyStatus[] m_DroneEnemyStatus;

    [SerializeField]
    private DroneState[] m_DroneEnemyState;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public DroneEnemyStatus[] GetDroneStatus()
    {
        return m_DroneEnemyStatus;
    }

    public DroneState[] GetDroneState()
    {
        return m_DroneEnemyState;
    }

}
