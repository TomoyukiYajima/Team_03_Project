using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour {
    [SerializeField] private float m_rayDist;
    [SerializeField] private CameraManager m_cameraManager;

    [SerializeField] private GameObject m_colliderObj;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward * m_rayDist);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_rayDist);

        if(m_colliderObj != hitInfo.collider)
        {
            if(m_colliderObj != null)
            {
                var materials = m_colliderObj.GetComponent<MeshRenderer>().materials;
                for (int i = 0; i != materials.Length; ++i)
                {
                    materials[i].DisableKeyword("_EMISSION");
                }

            }

            if (hitInfo.collider != null)
            {
                m_colliderObj = hitInfo.collider.gameObject;

                var materials = m_colliderObj.GetComponent<MeshRenderer>().materials;
                for (int i = 0; i != materials.Length; ++i)
                {
                    materials[i].EnableKeyword("_EMISSION");
                    materials[i].SetColor("_EmissionColor", new Color(1.0f, 1.0f, 1.0f));
                }

            }
            else
            {
                m_colliderObj = null;
            }

        }

        if (hitInfo.collider == null) return;

        if (hitInfo.collider.tag == "Camera" || hitInfo.collider.tag == "Robot")
        {
            var mesh = hitInfo.collider.gameObject.GetComponent<MeshRenderer>().materials;

            for (int i = 0; i != mesh.Length; ++i)
            {
                mesh[i].EnableKeyword("_EMISSION");
                mesh[i].SetColor("_EmissionColor", new Color(0.5f,0.5f,0.5f));
            }

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
