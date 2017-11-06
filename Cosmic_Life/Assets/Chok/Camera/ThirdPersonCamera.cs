using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour{
    [SerializeField, Tooltip("高さ調整")]           private float m_height      = 1.0f;
    [SerializeField, Tooltip("ターゲットとの距離")] private float m_distance    = 2.0f;
    [SerializeField, Tooltip("感度")]               private float m_sensitivity = 100.0f;
    [SerializeField, Tooltip("横ずらし")]           private float m_slide       = 0.0f;

    private GameObject m_target;

    // Use this for initialization
    void Start()
    {
        // ターゲットをプレイヤーに設定します
        m_target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // コントローラー右スティックで回転
        //var x = Input.GetAxis("HorizontalR") * Time.deltaTime * m_sensitivity;
        var y = Input.GetAxis("VerticalR")   * Time.deltaTime * m_sensitivity;
        //x = Input.GetAxis("HorizontalD");
        //y = Input.GetAxis("VerticalD");
        //x = Input.GetAxis("Trigger");
        //y = Input.GetAxis("Trigger");

        // 中心点を設定します
        var lookAt = m_target.transform.position + Vector3.up * m_height;

        // ターゲットの
        //transform.RotateAround(lookAt, Vector3.up, x);

        if (transform.forward.y >  0.3f && y < 0.0f) y = 0.0f;
        if (transform.forward.y < -0.9f && y > 0.0f) y = 0.0f;

        transform.RotateAround(lookAt, transform.right, y);

        Vector3 dir = lookAt - transform.position;
        dir.y = 0;
        dir.Normalize();

        Vector3 target = lookAt - transform.forward * m_distance;

        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(lookAt, target, out wallHit,1 << 8))
        {
            target = new Vector3(wallHit.point.x, target.y, wallHit.point.z);
        }

        transform.position = target;

        transform.LookAt(lookAt);

        transform.position = transform.position + transform.right * m_slide;

        //transform.Rotate(y, x, 0);

        //transform.position = m_target.transform.position - transform.forward * 2.0f;
        //transform.Translate(0, 1.0f, 0);

        //Ray ray = new Ray(transform.position, m_target.transform.position - transform.position);
        //RaycastHit hitInfo;

        //Ray rayBack = new Ray(transform.position, transform.position - m_target.transform.position);
        //Physics.Raycast(rayBack, out hitInfo, 1.25f);
        //if (hitInfo.collider == null)
        //{
        //    m_distance += 0.1f;
        //    m_distance = Mathf.Min(m_distance, 1.25f);
        //}


        //Physics.Raycast(ray, out hitInfo, 10.0f);

        //if (hitInfo.collider.tag != "Player")
        //{
        //    m_distance -= 0.1f;
        //}


        //if(Physics.Raycast(transform.position,m_target.transform.position - transform.position,out hitInfo, (m_target.transform.position - transform.position).magnitude,LayerMask.NameToLayer("WallLayer")))
        //{
        //    transform.position = (hitInfo.point - m_target.transform.position) * 0.8f + m_target.transform.position;
        //}
    }

}
