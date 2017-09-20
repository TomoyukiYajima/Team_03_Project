using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChecker : MonoBehaviour
{
    // チェックする半径
    [SerializeField]
    private float m_CheckRadius = 10.0f;
    // ステージオブジェクト格納リスト
    private List<GameObject> m_StageObjects =
        new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        var col = this.GetComponent<SphereCollider>();
        col.radius = m_CheckRadius;

        m_StageObjects.Clear();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // オブジェクト捜索範囲を取得します
    public float GetLength() { return m_CheckRadius; }

    // 範囲内にあるステージオブジェクトを返します
    public List<GameObject> GetStageObjects() { return m_StageObjects; }

    // 衝突した瞬間(トリガー用)
    public void OnTriggerEnter(Collider other)
    {
        var stageObj = other.gameObject.GetComponent<StageObject>();
        // ステージオブジェクト以外なら返す
        if (stageObj == null) return;
        //stageObj.EnableEmission(new Color(0.5f, 0.5f, 0.5f));
        stageObj.FlashEmission(new Color(0.5f, 0.5f, 0.5f), 0.5f);
        // 配列に追加
        m_StageObjects.Add(other.gameObject);
    }

    // 衝突終了後(トリガー用)
    public void OnTriggerExit(Collider other)
    {
        var stageObj = other.gameObject.GetComponent<StageObject>();
        // ステージオブジェクト以外なら返す
        if (stageObj == null) return;
        //stageObj.DisableEmission();
        stageObj.EndFlashEmission();
        // 配列から削除
        for (int i = 0; i != m_StageObjects.Count; ++i)
        {
            if (m_StageObjects[i] != stageObj.gameObject) continue;
            // 同一のオブジェクトだったら、配列から削除する
            m_StageObjects.Remove(stageObj.gameObject);
            break;
        }
    }

    // ギズモの表示
    public void OnDrawGizmos()
    {
        // ギズモの色の変更
        var color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        // 円の表示
        Gizmos.DrawSphere(this.transform.position, m_CheckRadius);
    }
}
