using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickJaming : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Robot") return;

        // ロボットを止める

        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
