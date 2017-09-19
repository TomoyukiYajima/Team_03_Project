using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour {
    [SerializeField] private float m_rayDist;
    [SerializeField] private CameraManager m_cameraManager;

	// Use this for initialization
	void Start () {
        m_cameraManager = GetComponent<CameraManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward * m_rayDist);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_rayDist);
        if (hitInfo.collider.tag == "Camera")
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                m_cameraManager.SwitchCamera(hitInfo.collider.gameObject);
            }
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawRay(transform.position, transform.forward * m_rayDist);
    }


}
