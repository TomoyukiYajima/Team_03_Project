//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Worker : MonoBehaviour, IRobotEvent
{

    //public enum OrderOption {
    //    NONE        = 1 << 0,
    //    DIRECTION   = 1 << 1
    //}

    // ワーカーの名前
    [SerializeField]
    private string m_WorkerName = "Worker";

    // 接地しているか
    private bool m_IsGround = false;
    // ジャミングされているか？
    private bool m_IsJamming = false;

    // 命令
    protected Dictionary<OrderNumber, OrderStatus> m_OrderStatus =
        new Dictionary<OrderNumber, OrderStatus>();

    // 命令リストのリスト1
    private Dictionary<OrderStatus, Order> m_OrdersOne =
        new Dictionary<OrderStatus, Order>();
    // 命令リストのリスト2
    private Dictionary<OrderStatus, Order> m_OrdersTwo =
        new Dictionary<OrderStatus, Order>();
    // 命令リストのリスト3
    private Dictionary<OrderStatus, Order> m_OrdersThree =
        new Dictionary<OrderStatus, Order>();
    // 命令リストのリスト
    private Dictionary<OrderNumber, Dictionary<OrderStatus, Order>> m_Orders =
        new Dictionary<OrderNumber, Dictionary<OrderStatus, Order>>();

    // 命令リストオブジェクト
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
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // ジャミング状態なら返す
        if (m_IsJamming) return;

        // デルタタイムの取得
        float time = Time.deltaTime;
        // 命令の実行
        m_Orders[OrderNumber.ONE][m_OrderStatus[OrderNumber.ONE]].Action(time, gameObject);
        m_Orders[OrderNumber.TWO][m_OrderStatus[OrderNumber.TWO]].Action(time, gameObject);
        m_Orders[OrderNumber.THREE][m_OrderStatus[OrderNumber.THREE]].Action(time, gameObject);

        // 命令が終了していれば、NULLの状態に変更する
        //if (m_Orders[m_OrderState].IsEndOrder()) ChangeOrder(OrderStatus.NULL);

        // 命令(仮)　音声認識でプレイヤーから命令してもらう
        // OKボタンが押されたら、移動命令を行う
        if (PlayerInputManager.GetInputDown(InputState.INPUT_OK)) ChangeOrder(OrderStatus.MOVE);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_CANCEL)) ChangeOrder(OrderStatus.STOP);

        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) ChangeOrder(OrderStatus.TURN_LEFT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) ChangeOrder(OrderStatus.TURN_RIGHT);

        // 持ち上げサンプル
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.LIFT);
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.PULL_OUT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.TAKE_DOWN);

        // 攻撃サンプル
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.MOVE, OrderDirection.RIGHT);
        if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.ATTACK_HIGH);
        //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.ATTACK_LOW);

        m_StateTimer += time;

        m_Rigidbody.velocity = Vector3.zero;

        // 一定時間命令がなかったら、寝そべる
        if (m_StateTimer >= 20.0f)
        {

        }

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
        // 1
        for (int i = 0; i != m_OrderList.GetOrderStatus(OrderNumber.ONE).Length; ++i)
        {
            var orders = m_OrderList.GetOrders(OrderNumber.ONE)[i];
            m_OrdersOne.Add(m_OrderList.GetOrderStatus(OrderNumber.ONE)[i], orders);

            //m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime, gameObj) => { orders.Action(deltaTime, gameObj); });
            // 下記だと、iの値が変に加算されてしまうので注意！
            // m_Orders.Add(m_OrderList.GetOrderStatus()[i], (deltaTime) => { m_OrderList.GetOrders()[i].Action(); });
        }
        // 2
        for (int i = 0; i != m_OrderList.GetOrderStatus(OrderNumber.TWO).Length; ++i)
        {
            var orders = m_OrderList.GetOrders(OrderNumber.TWO)[i];
            m_OrdersTwo.Add(m_OrderList.GetOrderStatus(OrderNumber.TWO)[i], orders);
        }
        // 3
        for (int i = 0; i != m_OrderList.GetOrderStatus(OrderNumber.THREE).Length; ++i)
        {
            var orders = m_OrderList.GetOrders(OrderNumber.THREE)[i];
            m_OrdersThree.Add(m_OrderList.GetOrderStatus(OrderNumber.THREE)[i], orders);
        }
        // 追加
        m_Orders.Add(OrderNumber.ONE, m_OrdersOne);
        m_Orders.Add(OrderNumber.TWO, m_OrdersTwo);
        m_Orders.Add(OrderNumber.THREE, m_OrdersThree);
        // 追加
        m_OrderStatus.Add(OrderNumber.ONE, OrderStatus.NULL);
        m_OrderStatus.Add(OrderNumber.TWO, OrderStatus.NULL);
        m_OrderStatus.Add(OrderNumber.THREE, OrderStatus.NULL);
    }

    // public virtual void ChangeOrder(OrderStatus order, OrderNumber number = OrderNumber.ONE)
    // 命令の変更を行います
    public virtual void ChangeOrder(OrderStatus order)
    {
        // 命令のあるオーダー番号の捜索
        OrderNumber number = OrderNumber.ONE;
        if (m_OrderList.IsOrder(OrderNumber.TWO, order)) number = OrderNumber.TWO;
        else if (m_OrderList.IsOrder(OrderNumber.THREE, order)) number = OrderNumber.THREE;

        // 命令がない場合は返す
        if (!CheckrOrder(order, number) || m_OrderStatus[number] == order) return;

        //// 同じ命令の場合
        //if(m_OrderState == order)
        //{
        //    // 命令オプションも同一の場合は返す
        //    if (m_OrderOption == option) return;
        //}

        print("命令承認！");

        // 最後の行動
        //m_OrdersOne[m_OrderOneState].EndAction();
        m_Orders[number][m_OrderStatus[number]].EndAction();

        // 命令状態の変更
        //m_OrderOneState = order;
        m_OrderStatus[number] = order;
        m_StateTimer = 0.0f;

        // 最初の行動
        // m_OrdersOne[m_OrderOneState].StartAction(gameObject);
        m_Orders[number][m_OrderStatus[number]].StartAction(gameObject);
    }

    // public virtual void ChangeOrder(OrderStatus order, OrderDirection dir, OrderNumber number = OrderNumber.ONE)
    public virtual void ChangeOrder(OrderStatus order, OrderDirection dir)
    {
        // 命令のあるオーダー番号の捜索
        OrderNumber number = OrderNumber.ONE;
        if (m_OrderList.IsOrder(OrderNumber.TWO, order)) number = OrderNumber.TWO;
        else if (m_OrderList.IsOrder(OrderNumber.THREE, order)) number = OrderNumber.THREE;

        // 命令がない場合は返す
        if (!CheckrOrder(order, number) || m_OrderStatus[number] == order) return;

        print("方向指定命令承認！");

        // 最後の行動
        m_Orders[number][m_OrderStatus[number]].EndAction();

        // 命令状態の変更
        m_OrderStatus[number] = order;
        m_StateTimer = 0.0f;

        // 方向指定の最初の行動
        //m_Orders[m_OrderState].StartAction(gameObject);
        m_Orders[number][m_OrderStatus[number]].GetComponent<DirectionOrder>().StartAction(gameObject, dir);
    }

    //// イベントでの呼び出し
    //public void onOrder(OrderStatus order)
    //{
    //    ChangeOrder(order);
    //}

    // イベントでの呼び出し(方向指定)
    public void onOrder(OrderStatus order, OrderDirection dir)
    {
        ChangeOrder(order, dir);
    }

    // 指定した命令があるかの確認を行います
    protected bool CheckrOrder(OrderStatus order, OrderNumber number)
    {
        // 命令
        for (int i = 0; i != m_OrderList.GetOrderStatus(number).Length; ++i)
        {
            var orderState = m_OrderList.GetOrderStatus(number)[i];
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

        //// 最後の行動
        //m_OrdersOne[m_OrderOneState].EndAction();

        //// 命令状態をNULLにする
        //m_OrderOneState = OrderStatus.NULL;
        //m_StateTimer = 0.0f;

        //// 最初の行動
        //m_OrdersOne[m_OrderOneState].StartAction(gameObject);
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
