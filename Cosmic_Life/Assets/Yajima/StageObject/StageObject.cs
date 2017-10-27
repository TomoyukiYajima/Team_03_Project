using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageObject : MonoBehaviour
{

    // 子オブジェクト
    [SerializeField]
    private GameObject m_Child;

    // 元の親オブジェクト
    private Transform m_RootParent;
    // 子オブジェクトのマテリアル配列
    private Material[] m_Materials;
    // 色の初期値配列
    private Color[] m_Colors;
    // 剛体
    private Rigidbody m_Rigidbody;

    // 衝突する前の座標
    private Vector3 m_PravPosition;
    // 衝突するまでの回転量
    private Quaternion m_PravRotate;
    // 衝突するまでの移動量
    private Vector3 m_PravVelocity;
    // ステージオブジェクトに衝突しているか
    private bool m_IsStageObjectHit = false;
    // 衝突しているオブジェクト
    private List<GameObject> m_HitObjects = new List<GameObject>();
    // 衝突していたオブジェクト
    //private List<GameObject> m_PrevHitObjects = new List<GameObject>();

    // 点滅するか
    private bool m_IsFlash = false;
    // 接地しているか
    private bool m_isGround = false;

    // Use this for initialization
    void Start()
    {
        // 親オブジェクトを入れる
        m_RootParent = this.transform.parent;

        // モデルを使用する場合は、こっちを適用する
        //var mesh = m_Child.GetComponent<MeshRenderer>();
        //m_Materials = mesh.materials;

        // モデルなしバージョン
        m_Materials = this.GetComponent<MeshRenderer>().materials;
        // モデルありバージョン
        //for(int i = 0; i != m_Materials.Length; ++i)
        //{
        //    m_Colors[i] = m_Materials[i].color;
        //}

        m_Rigidbody = this.GetComponent<Rigidbody>();

        //m_PrevPosition = this.transform.position;
        m_PravPosition = this.transform.position;
        m_PravRotate = this.transform.rotation;
        m_PravVelocity = m_Rigidbody.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = m_PrevPosition;

        m_isGround = false;

        //m_Rigidbody.velocity = Vector3.zero;
        //m_PrevPosition = this.transform.position;
        m_PravPosition = this.transform.position;
        m_PravRotate = this.transform.rotation;
        m_PravVelocity = m_Rigidbody.velocity;

        if (this.transform.parent == null) return;
        // アンドロイドがオブジェクトを持っていない場合
        if(this.transform.parent.name != "LiftObject")
        {
            if (m_Rigidbody.velocity.x == 0.0f && m_Rigidbody.velocity.z == 0.0f && m_IsStageObjectHit == false)
            {
                //m_Rigidbody.isKinematic = true;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
        }

        //if (m_PrevHitObjects.Count == 0) return;

        //ReleasObject();
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
    }

    // 参照しているオブジェクトの解放
    private IEnumerator ReleasObject(GameObject obj)
    {
        yield return new WaitForSeconds(Time.deltaTime);

        m_HitObjects.Remove(obj);
        //// 削除するオブジェクトを捜して削除する
        //for (int i = 0; i != m_PrevHitObjects.Count; ++i)
        //{
        //    GameObject obj = m_PrevHitObjects[i];
        //    for (int j = 0; j != m_HitObjects.Count; ++i)
        //    {
        //        if (obj != m_HitObjects[j]) continue;
        //        // 同一のオブジェクト
        //        m_HitObjects.Remove(obj);
        //        m_PrevHitObjects.Remove(obj);
        //        // 削除するオブジェクトがない場合はbreakする
        //        if (m_PrevHitObjects.Count == 0)
        //            break;
        //    }
        //    if (m_PrevHitObjects.Count == 0)
        //        break;
        //}
    }

    private void AddObject(GameObject obj)
    {
        //yield return new WaitForSeconds(Time.deltaTime);

        // if (m_HitObjects.IndexOf(obj) != -1) return;
        if (m_HitObjects.IndexOf(obj) < 0) m_HitObjects.Add(obj);
    }

    // 参照しているオブジェクトのクリア
    public void ClearObject()
    {
        //m_HitObjects.Clear();
        //m_PrevHitObjects.Clear();
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
        for (int i = 0; i != m_Materials.Length; ++i)
        {
            m_Materials[i].color = m_Colors[i];
        }
    }

    // オブジェクトと衝突しているかを返します
    public bool IsHit() { return m_HitObjects.Count != 0; }

    #region Unity関数
    // 衝突判定
    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.transform.tag == "Player")
        //{
        //    ////if (m_Rigidbody.isKinematic) return;
        //    //print(m_PravVelocity.ToString());
        //    ////m_Rigidbody.isKinematic = true;
        //    //// m_Rigidbody.velocity = Vector3.zero;
        //    //this.transform.position = m_PravPosition + m_PravVelocity * Time.deltaTime;
        //    //this.transform.rotation = m_PravRotate;
        //    //m_Rigidbody.velocity = m_PravVelocity;
        //}

        if(collision.transform.tag == "StageObject")
        {
            //m_PravPosition = this.transform.position;
            //m_PravRotate = this.transform.rotation;
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            m_IsStageObjectHit = true;
        }
        //else if (collision.transform.tag == "Sample")
        //{
        //    AddObject(collision.gameObject);
        //    print("Hit");
        //}
    }

    public void OnCollisionStay(Collision collision)
    {
        //if (collision.transform.tag == "Player")
        //{
        //    //if (m_Rigidbody.isKinematic) return;

        //    //m_Rigidbody.isKinematic = true;
        //    //m_Rigidbody.velocity = Vector3.zero;
        //    this.transform.position = m_PravPosition + m_PravVelocity * Time.deltaTime;
        //    this.transform.rotation = m_PravRotate;
        //    m_Rigidbody.velocity = m_PravVelocity;
        //}

        //if (collision.transform.tag == "StageObject")
        //{
        //    //m_PravPosition = this.transform.position;
        //    //m_PravRotate = this.transform.rotation;
        //    m_Rigidbody.constraints = RigidbodyConstraints.None;
        //}
    }

    public void OnCollisionExit(Collision collision)
    {

        if (collision.transform.tag == "StageObject")
        {
            m_IsStageObjectHit = false;
        }
        //else if (collision.transform.tag == "Sample")
        //{
        //    ReleasObject(collision.gameObject);
        //    //m_PrevHitObjects.Add(collision.gameObject);
        //}
        //if (collision.transform.tag == "Player") m_Rigidbody.isKinematic = false;
    }
    #endregion
}
