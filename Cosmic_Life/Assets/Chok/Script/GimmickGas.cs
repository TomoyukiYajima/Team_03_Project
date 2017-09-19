using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickGas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーじゃなければreturn
        if (other.tag != "Player") return;
        // プレイヤーを消す
        Destroy(other.gameObject);
    }


}
