using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerState
{
    NULL = 1 << 0,
    IDLE = 1 << 1,
    ATTACK = 1 << 2,
}


public class Player : MonoBehaviour,IGeneralEvent
{

    [SerializeField, Tooltip("移動速度")] private float m_Speed;
    [SerializeField, Tooltip("攻撃判定")] private GameObject m_attackCollision;
    [SerializeField, Tooltip("攻撃生成位置")] private GameObject m_attackPos;

    private Transform m_camera;
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private PlayerState m_state;
    private Vector3 m_velocity;

    private bool m_isDamaged;

    // Use this for initialization
    void Start()
    {
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


        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_state = PlayerState.IDLE;
        m_velocity = Vector3.zero;
        m_isDamaged = false;

        ChangeState(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("X")) { onDamage(0); }

        m_animator.SetFloat("Forward", m_velocity.z, 0.1f, Time.deltaTime);
        
    }

    private void UpdateState()
    {
        //switch (m_state)
        //{
        //    case PlayerState.IDLE:
        //        break;
        //    case PlayerState.ATTACK:
        //        Attack();
        //        break;
        //    default:
        //        break;
        //}
    }

    private void ChangeState(IEnumerator coroutine)
    {
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

    private void ToMoveState()
    {
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            // デルタタイムの取得
            float time = Time.deltaTime;

            m_velocity = Vector3.zero;

            if (Input.GetButtonDown("OK"))
            {
                ToAttackState();
                yield return null;
            }

            float x = Input.GetAxis("Vertical");
            float z = Input.GetAxis("Horizontal");

            Vector3 velocity = Vector3.zero;

            //velocity = new Vector3(x, 0, z);
            if (m_camera != null)
            {
                var forward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;
                m_velocity += forward * x;
                m_velocity += m_camera.right * z;
            }

            if (m_velocity.magnitude > 1f) m_velocity.Normalize();
            m_velocity = transform.InverseTransformDirection(m_velocity);

            float m_TurnAmount = Mathf.Atan2(m_velocity.x, m_velocity.z);

            float turnSpeed = Mathf.Lerp(180.0f, 360.0f, m_velocity.z);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

            Vector3 v = (m_animator.deltaPosition * m_Speed) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_rigidbody.velocity.y;
            m_rigidbody.velocity = v;

            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0, m_rigidbody.velocity.z);

            //this.transform.position += m_velocity * m_Speed * time;

            m_animator.SetFloat("Forward", m_velocity.z, 0.1f, Time.deltaTime);

            yield return null;
        }
    }

    private void ToAttackState()
    {
        // コールチンを停止
        StopAllCoroutines();

        // 状態変更
        //m_state = PlayerState.ATTACK;
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        m_rigidbody.velocity = Vector3.zero;
        // 攻撃コリジョン生成
        GameObject attack = Instantiate(m_attackCollision, m_attackPos.transform.position, m_attackPos.transform.rotation) as GameObject;
        DestroyObject(attack, 0.5f);
        // モーション変更

        yield return new WaitForSeconds(1.0f);
        ToMoveState();
        yield return null;
    }

    private IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DamageWait());
        StartCoroutine(DamageInput());

        yield return new WaitWhile(() => m_isDamaged);

        ToMoveState();

        yield return null;
    }

    private IEnumerator DamageInput()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        m_isDamaged = false;
    }

    private IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(5.0f);
        m_isDamaged = false;
    }

    public void onDamage(int amount)
    {
        m_velocity = Vector3.zero;

        StopAllCoroutines();
        m_isDamaged = true;

        StartCoroutine(Damage());
    }
}
