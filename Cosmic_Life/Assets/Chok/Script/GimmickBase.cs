using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBase : MonoBehaviour,IGimmickEvent {

    protected bool m_isActivated;

    // Use this for initialization
    void Start () {
        m_isActivated = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public virtual void onActivate(GameObject obj)
    {
    }

    public virtual void onActivate(string password)
    {
    }

    public virtual void onActivate()
    {
    }
}
