using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateList : MonoBehaviour {

    [SerializeField]
    private EnemyStatus[] m_EnemyStatus;

    [SerializeField]
    private EnemyState[] m_EnemyState;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public EnemyStatus[] GetOrderStatus()
    {
        return m_EnemyStatus;
    }

    public EnemyState[] GetEnemyOrder()
    {
        return m_EnemyState;
    }
}
