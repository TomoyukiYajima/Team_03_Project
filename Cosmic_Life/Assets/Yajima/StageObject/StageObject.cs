using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour {

    // 子オブジェクト
    [SerializeField]
    private GameObject m_Child;

    // 元の親オブジェクト
    private Transform m_RootParent;
    // 子オブジェクトのマテリアル配列
    private Material[] m_Materials;

	// Use this for initialization
	void Start () {
        // 親オブジェクトを入れる
        m_RootParent = this.transform.parent;

        // モデルを使用する場合は、こっちを適用する
        //var mesh = m_Child.GetComponent<MeshRenderer>();
        //m_Materials = mesh.materials;

        // モデルなしバージョン
        m_Materials = this.GetComponent<MeshRenderer>().materials;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // 自己発光の設定を行います
    public void EnableEmission(Color color)
    {
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].EnableKeyword("_EMISSION");
            m_Materials[i].SetColor("_EmissionColor", color);
        }
    }

    // 自己発光をオフにします
    public void DisableEmission()
    {
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].DisableKeyword("_EMISSION");
        }
    }

    // 現在の親を元の親に初期化します
    public void InitParent()
    {
        this.transform.parent = m_RootParent;
    }
}
