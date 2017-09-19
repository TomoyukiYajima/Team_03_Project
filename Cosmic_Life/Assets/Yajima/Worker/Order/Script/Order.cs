using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour {

    // 命令が終了したか
    protected bool m_IsEndOrder = false;

	// Use this for initialization
	public virtual void Start () {
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}

    // 最初の行動
    public virtual void StartAction(GameObject obj)
    {
    }

    // 行動
    public virtual void Action(float deltaTime, GameObject obj)
    {
    }

    // 行動終了
    public virtual void EndAction()
    {
        m_IsEndOrder = false;
    }

    // 命令が終了したかを返します
    public bool IsEndOrder() { return m_IsEndOrder; }
}
