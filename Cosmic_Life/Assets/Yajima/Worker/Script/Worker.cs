//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Worker : MonoBehaviour, IRobotEvent
{

    // ワーカーの名前
    [SerializeField]
    private string m_WorkerName = "Worker";

    // 接地しているか
    private bool m_IsGround = false;
    // ジャミングされているか？
    private bool m_IsJamming = false;

    // 命令
    protected OrderStatus m_OrderState = OrderStatus.NULL;
    // 命令格納コンテナ
    //private Dictionary<OrderStatus, Action<float, GameObject>> m_Orders =
    //    new Dictionary<OrderStatus, Action<float, GameObject>>();
    private Dictionary<OrderStatus, Order> m_Orders =
        new Dictionary<OrderStatus, Order>();

    private Dictionary<OrderStatus, Dictionary<DirectionNumber, Order>> m_Hoge =
        new Dictionary<OrderStatus, Dictionary<DirectionNumber, Order>>();

    private Dictionary<DirectionNumber, Order> m_Piyo =
        new Dictionary<DirectionNumber, Order>();

    // 命令リスト
    protected OrderList m_OrderList;
    // 命令実行時間
    protected float m_StateTimer = 0.0f;

    // 剛体
    protected Rigidbody m_Rigidbody;

    // Use this for initialization
    public virtual void Start()
    {
        // 命令の設定
        SetOrder();

        m_Rigidbody = this.GetComponent<Rigidbody>();

        m_Hoge.Add(OrderStatus.ATTACK, m_Piyo);
        m_Hoge[OrderStatus.ATTACK][DirectionNumber.FORWARD].Action(1.0f, gameObject);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // ジャミング状態なら返す
        if (m_IsJamming) return;

        // デルタタイムの取得
        float time = Time.deltaTime;
        // 命令の実行
        //m_Orders[m_OrderState](time, gameObject);
        m_Orders[m_OrderState].Action(time, gameObject);
        // 命令が終了していれば、NULLの状態に変更する
        //if (m_Orders[m_OrderState].IsEndOrder()) ChangeOrder(OrderStatus.NULL);

        // 命令(仮)　音声認識でプレイヤーから命令してもらう
        // OKボタンが押されたら、移動命令を行う
        if (PlayerInputManager.GetInputDown(InputState.INPUT_OK)) ChangeOrder(OrderStatus.MOVE);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_CANCEL)) ChangeOrder(OrderStatus.STOP);

        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) ChangeOrder(OrderStatus.TURN_LEFT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) ChangeOrder(OrderStatus.TURN_RIGHT);

        // 持ち上げサンプル
        if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.LIFT);
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.PULL_OUT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.TAKE_DOWN);

        // 攻撃サンプル
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.ATTACK_HIGH);
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.ATTACK_LOW);

        m_StateTimer += time;

        m_Rigidbody.velocity = Vector3.zero;

        // 接地していない場合は、重力加算
        //if (!m_IsGround)
        //{
        //    this.transform.position += Vector3.down * 9.8f * time;
        //}
    }

    // 命令の設定を行います
    protected virtual void SetOrder()
    {
        // 命令リストの取得
        m_OrderList = this.transform.Find("OrderList").GetComponent<OrderList>();

        // 命令の追加
        for (int i = 0; i != m_OrderList.GetOrderStatus().Length; ++i)
        {
            var orders = m_OrderList.GetOrders()[i];
            m_Orders.Add(m_OrderList.GetOrderStatus()[i], orders);

            //m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime, gameObj) => { orders.Action(deltaTime, gameObj); });
            // 下記だと、iの値が変に加算されてしまうので注意！
            // m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime) => { m_OrderList.GetOrders()[i].Action(); });
        }
    }

    // 命令の変更を行います
    public virtual void ChangeOrder(OrderStatus order)
    {
        // 命令がない場合は返す
        if (!CheckrOrder(order) || m_OrderState == order) return;

        print("命令承認！");

        // 最後の行動
        m_Orders[m_OrderState].EndAction();

        // 命令状態の変更
        m_OrderState = order;
        m_StateTimer = 0.0f;

        // 最初の行動
        m_Orders[m_OrderState].StartAction(gameObject);
    }

    public void onOrder(OrderStatus order, OrderDirection dir)
    {
        ChangeOrder(order);
    }

    // 指定した命令があるかの確認を行います
    protected bool CheckrOrder(OrderStatus order)
    {
        // 命令の追加
        for (int i = 0; i != m_OrderList.GetOrderStatus().Length; ++i)
        {
            var orderState = m_OrderList.GetOrderStatus()[i];
            // 同一の命令だった場合はtrueを返す
            if (order == orderState) return true;
        }
        // 同一の命令がない
        return false;
    }

    // ジャミングかどうか
    public void Jamming(bool isJamming)
    {
        if (isJamming) Jamming();
        else NotJamming();
    }

    // ジャミング用
    private void Jamming()
    {
        m_IsJamming = true;

        // 最後の行動
        m_Orders[m_OrderState].EndAction();

        // 命令状態をNULLにする
        m_OrderState = OrderStatus.NULL;
        m_StateTimer = 0.0f;

        // 最初の行動
        m_Orders[m_OrderState].StartAction(gameObject);
    }

    // ジャミング解除用
    private void NotJamming()
    {
        m_IsJamming = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // 地面との判定
        if (collision.transform.tag != "Ground") return;
        m_IsGround = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        // 地面との判定
        if (collision.transform.tag != "Ground") return;
        m_IsGround = false;
    }


    //    #region エディターのシリアライズ変更
    //    // 変数名を日本語に変換する機能
    //    // CustomEditor(typeof(Enemy), true)
    //    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
    //#if UNITY_EDITOR
    //    [CustomEditor(typeof(Worker), true)]
    //    [CanEditMultipleObjects]
    //    public class WorkerEditor : Editor
    //    {
    //        SerializedProperty DelayTimer;

    //        public void OnEnable()
    //        {
    //            DelayTimer = serializedObject.FindProperty("m_DelayTimer");
    //        }

    //        public override void OnInspectorGUI()
    //        {
    //            // 更新
    //            serializedObject.Update();

    //            // 自身の取得;
    //            Worker worker = target as Worker;

    //            // エディタ上でのラベル表示
    //            EditorGUILayout.LabelField("〇ワーカーステータス");

    //            // float
    //            DelayTimer.floatValue = EditorGUILayout.FloatField("命令遅延時間", worker.m_DelayTimer);

    //            //EditorGUILayout.Space();

    //            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
    //            serializedObject.ApplyModifiedProperties();
    //        }
    //    }
    //#endif

    //    #endregion
}
