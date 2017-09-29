using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float m_Speed;  // 移動速度

    private Transform m_camera;

	// Use this for initialization
	void Start () {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_camera = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

    }

    // Update is called once per frame
    void Update () {
        // デルタタイムの取得
        float time = Time.deltaTime;

        float x = PlayerInputManager.GetVertical();
        float z = PlayerInputManager.GetHorizontal();

        Vector3 velocity = Vector3.zero;

        //velocity = new Vector3(x, 0, z);
        if (m_camera != null)
        {
            var forward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;


            velocity += forward * x;
            velocity += m_camera.right * z;
        }

        this.transform.position += velocity * m_Speed * time;
	}
}
