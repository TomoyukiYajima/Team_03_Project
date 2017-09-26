using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private GameObject m_target;

	// Use this for initialization
	void Start () {
        m_target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float x = 0.0f;
        float y = 0.0f;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("RVertical");


        transform.Rotate(y, x, 0);
    }
}
