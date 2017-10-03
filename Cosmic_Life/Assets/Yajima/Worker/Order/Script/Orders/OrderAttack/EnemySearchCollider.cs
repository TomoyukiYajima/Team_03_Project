using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchCollider : MonoBehaviour
{

    // 範囲内に居る敵
    private List<GameObject> m_Enemys = new List<GameObject>();

    // Use this for initialization
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // 索敵範囲内にいる敵を返します
    public List<GameObject> GetEnemys() { return m_Enemys; }

    public void OnTriggerEnter(Collider other)
    {
        // 相手のタグが"Enemy"以外 または、リストに入っていたら返す
        if (other.tag != "Enemy" || m_Enemys.Contains(other.gameObject)) return;
        // リストに追加
        m_Enemys.Add(other.gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        // リストに入っていなかったら、返す
        if (!m_Enemys.Contains(other.gameObject)) return;
        // リストから除外する
        m_Enemys.Remove(other.gameObject);
    }
}
