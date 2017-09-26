using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoundEnemy : MonoBehaviour, IEnemyEvent {

    public NavMeshAgent m_SoundAgent;

	// Use this for initialization
	void Start () {
        m_SoundAgent = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onHear()
    {
        print("追跡!!!");

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= 100.0f)
        {
            m_SoundAgent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    public void onDamage(int amount)
    {

    }

}
