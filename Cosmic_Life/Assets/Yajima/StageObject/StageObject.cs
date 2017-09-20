using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageObject : MonoBehaviour {

    // 子オブジェクト
    [SerializeField]
    private GameObject m_Child;

    // 元の親オブジェクト
    private Transform m_RootParent;
    // 子オブジェクトのマテリアル配列
    private Material[] m_Materials;
    // 色の初期値配列
    private Color[] m_Colors;
    // 点滅するか
    private bool m_IsFlash = false;
    // 接地しているか
    private bool m_isGround = false;

	// Use this for initialization
	void Start () {
        // 親オブジェクトを入れる
        m_RootParent = this.transform.parent;

        // モデルを使用する場合は、こっちを適用する
        //var mesh = m_Child.GetComponent<MeshRenderer>();
        //m_Materials = mesh.materials;

        // モデルなしバージョン
        m_Materials = this.GetComponent<MeshRenderer>().materials;
        //for(int i = 0; i != m_Materials.Length; ++i)
        //{
        //    m_Colors[i] = m_Materials[i].color;
        //}
    }
	
	// Update is called once per frame
	void Update () {
        m_isGround = false;
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

    // Emissionの点滅を行います
    public void FlashEmission(Color color, float time)
    {
        m_IsFlash = true;

        StartCoroutine(Flash(color, time));
    }

    public IEnumerator Flash(Color color, float time)
    {
        // Emission をオンにする
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            // Tween で色変換
            m_Materials[i].EnableKeyword("_EMISSION");
            m_Materials[i].DOColor(color, "_EmissionColor", time);
            //m_Materials[i].SetColor("_EmissionColor", color);
        }

        // ディレイ
        yield return new WaitForSeconds(time);

        // Emission をオフにする
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].DOColor(new Color(0.0f, 0.0f, 0.0f), "_EmissionColor", time);
            //m_Materials[i].DisableKeyword("_EMISSION");
        }

        yield return new WaitForSeconds(time);

        // 再度点滅する場合
        if (m_IsFlash) StartCoroutine(Flash(color, time));
        else
        {
            for (int i = 0; i != m_Materials.Length; ++i)
            {
                m_Materials[i].DisableKeyword("_EMISSION");
            }
        }
    }

    // Emissionの点滅を終了させます
    public void EndFlashEmission()
    {
        m_IsFlash = false;
    }

    // 色のリセットを行います
    public void ResetColor()
    {
        // 初期値を代入する
        for(int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].color = m_Colors[i];
        }
    }
}
