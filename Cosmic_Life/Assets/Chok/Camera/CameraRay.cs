using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private float m_rayDist;
    [SerializeField] private CameraManager m_cameraManager;

    [SerializeField] private GameObject m_colliderObj;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward * m_rayDist);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, m_rayDist);

        if (hitInfo.collider == null)
        {
            if (m_colliderObj != null)
            {
                StageObject material = null;
                if ((material = m_colliderObj.GetComponent<StageObject>()) != null)
                {
                    material.EndFlashEmission();
                }
                m_colliderObj = null;
            }
            return;
        }
        else if (m_colliderObj != hitInfo.collider.gameObject)
        {
            m_colliderObj = hitInfo.collider.gameObject;

            StageObject material = null;
            if ((material = m_colliderObj.GetComponent<StageObject>()) != null)
            {
                material.FlashEmission(new Color(0.5f, 0.5f, 0.5f), 1.0f);
            }
        }

        if (hitInfo.collider == null) return;

        if (hitInfo.collider.tag == "Camera" || hitInfo.collider.tag == "Robot")
        {
            if (Input.GetKeyDown(KeyCode.J))
            {

                m_cameraManager.SwitchCamera(hitInfo.collider.gameObject.transform, 0.5f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawRay(transform.position, transform.forward * m_rayDist);
    }
}
