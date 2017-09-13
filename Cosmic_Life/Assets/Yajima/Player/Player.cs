using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float m_Speed;  // 移動速度

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // デルタタイムの取得
        float time = Time.deltaTime;

        Vector3 velocity = new Vector3(
            PlayerInputManager.GetHorizontal(), 
            0.0f,
            PlayerInputManager.GetVertical()
            ).normalized;
        this.transform.position += velocity * m_Speed * time;
	}
}
