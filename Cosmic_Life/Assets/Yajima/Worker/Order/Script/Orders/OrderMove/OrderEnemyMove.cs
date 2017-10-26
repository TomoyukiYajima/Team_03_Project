using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEnemyMove : Order {

    // 敵捜索オブジェクト
    [SerializeField]
    private EnemySearchCollider m_Collider;
    [SerializeField]
    private float m_Speed = 10.0f;
    // 攻撃する敵
    private GameObject m_Enemy;
    // 攻撃する角度
    private float m_AttackAngle = 5.0f;

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj, GameObject actionObj)
    {
        base.StartAction(obj);
        // 相手との距離を求める
        m_Enemy = GetEnemy(obj);
    }

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        base.UpdateAction(deltaTime, obj);

        if(m_Enemy == null)
        {
            //EndOrder(obj);
            print("ない");
            return;
        }

        // 敵に向かって移動
        var dir = (
            new Vector2(m_Enemy.transform.position.x, m_Enemy.transform.position.z) - 
            new Vector2(obj.transform.position.x, obj.transform.position.z)).normalized;

        if(Vector3.Distance(obj.transform.position, m_Enemy.transform.position) < 0.0f)
        {
            // 攻撃
            ChangeOrder(obj, OrderStatus.ATTACK_HIGH);
            EndOrder(obj);
            return;
        }

        // 移動
        obj.transform.position += (Vector3)dir * m_Speed * deltaTime;


        //// 一定角度内ならば攻撃命令に変更
        //if (IsAttackAngle(obj, m_Enemy))
        //{
        //    ChangeOrder(obj, OrderStatus.ATTACK);
        //    //ChangeOrder(obj, OrderStatus.STOP);
        //}
    }

    // 相手との距離を求めます
    private GameObject GetEnemy(GameObject robot)
    {
        GameObject enemy = null;

        for (int i = 0; i != m_Collider.GetEnemys().Count; ++i)
        {
            // 敵が空の場合は入れる
            if (enemy == null)
            {
                enemy = m_Collider.GetEnemys()[i];
                continue;
            }
            // 一番距離が短い敵を入れる
            float length = Vector3.Distance(robot.transform.position, enemy.transform.position);
            float length2 = Vector3.Distance(robot.transform.position, m_Collider.GetEnemys()[i].transform.position);
            // 前回の値より小さい場合
            if (length > length2) enemy = m_Collider.GetEnemys()[i];
        }

        return enemy;
    }

    //// 攻撃角度内か返します
    //private bool IsAttackAngle(GameObject robot, GameObject enemy)
    //{
    //    // 敵がいない場合は返す
    //    if (enemy == null) return false;

    //    // 敵との最小角度を求める
    //    float angle = Vector2.Angle(
    //        new Vector2(robot.transform.position.x, robot.transform.position.z),
    //        new Vector2(enemy.transform.position.x, enemy.transform.position.z)
    //        );
    //    // 一定角度外なら返す
    //    if (angle >= m_AttackAngle) return false;
    //    // 角度内
    //    return true;
    //}
}
