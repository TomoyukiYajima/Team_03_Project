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

    public EnemyStatus[] GetEnemyStatus()
    {
        return m_EnemyStatus;
    }

    public EnemyState[] GetEnemyState()
    {
        return m_EnemyState;
    }
}
