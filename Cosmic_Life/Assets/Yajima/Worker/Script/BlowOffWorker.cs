using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サボりロボット
public class BlowOffWorker : Worker
{
    // メインカメラ
    [SerializeField]
    private GameObject m_MainCamera;
    // サボる時間
    [SerializeField]
    private float m_BlowOffTime;

    // 見えているか
    private bool m_IsRender = true;

    // プレイヤーが見ていない時間
    private float m_LockAtTimer = 0.0f;

    // サボるか
    private bool m_IsBlowOff = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        if (m_MainCamera == null) m_MainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!m_IsRender)
        {
            m_LockAtTimer = Mathf.Min(m_LockAtTimer + Time.deltaTime, m_BlowOffTime);
            if (m_LockAtTimer == m_BlowOffTime) m_IsBlowOff = true;
        }

        m_IsRender = false;
    }

    // カメラ判定関数
    public void OnWillRenderObject()
    {
        if (m_MainCamera.tag != "MainCamera") return;
        // 見えている
        m_IsRender = true;
    }
}
