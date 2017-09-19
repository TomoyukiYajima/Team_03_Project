using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : EnemyState {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Action(float deltaTime, Enemy obj)
    {
        print("何もしない");
    }

}
