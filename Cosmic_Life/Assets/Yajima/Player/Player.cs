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

        float x = PlayerInputManager.GetHorizontal();
        float z = PlayerInputManager.GetVertical();

        Vector3 velocity = new Vector3(
            PlayerInputManager.GetHorizontal(), 
            0.0f,
            PlayerInputManager.GetVertical()
            ).normalized;
        this.transform.position += velocity * m_Speed * time;
	}
}
