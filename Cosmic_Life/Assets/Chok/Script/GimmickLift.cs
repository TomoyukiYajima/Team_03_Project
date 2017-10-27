using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimmickLift : GimmickBase {
    [SerializeField] private float m_activateDuration;
    [SerializeField] private float m_waitDuration;
    [SerializeField] private Transform[] m_points;

    private int m_point;

	// Use this for initialization
	void Start () {
        m_point = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void onActivate()
    {
        if (m_isActivated) return;
        m_isActivated = true;
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        m_point = (m_point + 1) % m_points.Length;
        transform.DOMove(m_points[m_point].position, m_activateDuration);

        yield return new WaitForSeconds(m_activateDuration);

        // SEやEffectを出す
        yield return new WaitForSeconds(m_waitDuration);

        m_isActivated = false;

        yield return null;
    }

}
