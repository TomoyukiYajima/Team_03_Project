using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Worker : MonoBehaviour, IOrderEvent, IGeneralEvent
{
    #region 変数
    #region シリアライズ変数
    // ワーカーの名前
    [SerializeField]
    private string m_WorkerName = "Undroid";
    // 命令リストオブジェクト
    [SerializeField]
    protected OrderList m_OrderList;
    //private OrderList m_OrderList = null;
    // 移動速度
    [SerializeField]
    private float m_MoveSpeed = 5.0f;
    // 回転速度
    [SerializeField]
    private float m_RotateSpeed = 30.0f;
    // 投げる力
    [SerializeField]
    private float m_ThrowPowor = 10.0f;
    // デバックするか
    [SerializeField]
    private bool m_IsDebug = false;
    #endregion

    #region protected変数
    // 命令実行時間
    protected float m_StateTimer = 0.0f;
    // 参照するオブジェクト
    protected GameObject m_ActionObject;
    // 剛体
    protected Rigidbody m_Rigidbody;
    #endregion

    #region private変数
    // どこを向いているかを表示するオブジェクト
    private GameObject m_LookObject;
    // 方向
    private OrderDirection m_OrderDir = OrderDirection.FORWARD;
    // 接地しているか
    private bool m_IsGround = false;
    // ジャミングされているか？
    private bool m_IsJamming = false;
    #endregion

    #region 配列
    // マネージャ側にやらせる
    // 実行中の命令格納コンテナ
    protected Dictionary<OrderNumber, OrderStatus> m_OrderStatus =
        new Dictionary<OrderNumber, OrderStatus>();
    // ここを変更する
    // 命令リストのリスト1
    private Dictionary<OrderStatus, Order> m_OrdersOne =
        new Dictionary<OrderStatus, Order>();
    // 命令リストのリスト2
    private Dictionary<OrderStatus, Order> m_OrdersTwo =
        new Dictionary<OrderStatus, Order>();
    // 命令リストのリスト3
    //[SerializeField]
    private Dictionary<OrderStatus, Order> m_OrdersThree =
        new Dictionary<OrderStatus, Order>();
    // ここまで変更する
    // 命令リストのリスト
    private Dictionary<OrderNumber, Dictionary<OrderStatus, Order>> m_Orders =
        new Dictionary<OrderNumber, Dictionary<OrderStatus, Order>>();
    // 前回の命令のリスト
    private Dictionary<OrderNumber, OrderStatus> m_PrevOrders =
        new Dictionary<OrderNumber, OrderStatus>();
    // 命令列挙リスト
    private List<OrderNumber> m_OrderNumbers =
        new List<OrderNumber>();

    // 命令格納コンテナ
    //private Dictionary<OrderStatus, Action<float, GameObject>> m_Orders =
    //    new Dictionary<OrderStatus, Action<float, GameObject>>();
    // 命令変更関数格納リスト
    private List<Action<OrderNumber, OrderDirection, GameObject>> m_ChangeOrders =
        new List<Action<OrderNumber, OrderDirection, GameObject>>();
    //private List<Action<OrderStatus, OrderNumber, int>> m_ChangeOrders =
    //    new List<Action<OrderStatus, OrderNumber, int>>();
    #endregion
    #endregion

    #region 関数
    #region 基盤関数
    // Use this for initialization
    public virtual void Start()
    {
        // 命令の設定
        SetOrder();

        m_Rigidbody = this.GetComponent<Rigidbody>();

        m_LookObject = this.transform.Find("LookObject").gameObject;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // ジャミング状態なら返す
        if (m_IsJamming) return;

        // デルタタイムの取得
        float time = Time.deltaTime;
        // 命令の実行
        for(int i = 0; i != m_OrderNumbers.Count; ++i)
        {
            m_Orders[m_OrderNumbers[i]][m_OrderStatus[m_OrderNumbers[i]]].Action(time, gameObject);
        }

        // 命令(仮)　音声認識でプレイヤーから命令してもらう
        if (m_IsDebug)
        {
            // OKボタンが押されたら、移動命令を行う
            if (PlayerInputManager.GetInputDown(InputState.INPUT_OK)) ChangeOrder(OrderStatus.MOVE);
            if (PlayerInputManager.GetInputDown(InputState.INPUT_CANCEL)) ChangeOrder(OrderStatus.ALLSTOP);

            if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) ChangeOrder(OrderStatus.TURN, OrderDirection.LEFT);
            if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) ChangeOrder(OrderStatus.TURN, OrderDirection.RIGHT);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) ChangeOrder(OrderStatus.TURN, OrderDirection.LEFT);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) ChangeOrder(OrderStatus.TURN, OrderDirection.RIGHT);

            //// 持ち上げサンプル
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.LOOK, OrderDirection.UP);
            if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.LIFT);
            if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.LIFT_UP);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.LIFT_UP);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.ATTACK_MOW_DOWN);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.PULL_OUT);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.TAKE_DOWN);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) stopOrder(OrderStatus.ATTACK_HIGH);

            // 攻撃サンプル
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.MOVE, OrderDirection.RIGHT);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) ChangeOrder(OrderStatus.ATTACK_HIGH);
            //if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) ChangeOrder(OrderStatus.ATTACK_LOW);

        }

        if (m_OrderDir == OrderDirection.UP) m_LookObject.transform.localPosition = Vector3.up;
        else if (m_OrderDir == OrderDirection.DOWN) m_LookObject.transform.localPosition = Vector3.down;
        else if (m_OrderDir == OrderDirection.FORWARD) m_LookObject.transform.localPosition = Vector3.zero;
        //print(m_OrderDir.ToString());

        m_StateTimer += time;

        m_Rigidbody.velocity = new Vector3(0.0f, m_Rigidbody.velocity.y, 0.0f);

        // 一定時間命令がなかったら、寝そべる
        if (m_StateTimer >= 20.0f)
        {

        }

        // 接地していない場合は、重力加算
        if (!m_IsGround)
        {
            //this.transform.position += Vector3.down * 9.8f * time;
        }
    }
    #endregion

    #region 命令関数
    // 命令の設定を行います
    protected virtual void SetOrder()
    {
        // 命令リストの取得
        if (m_OrderList == null) m_OrderList = this.transform.Find("OrderList").GetComponent<OrderList>();
        m_OrderList.InitializeOrder();

        // m_OrderNumbers[i]
        m_OrderNumbers.Add(OrderNumber.ONE);
        m_OrderNumbers.Add(OrderNumber.TWO);
        m_OrderNumbers.Add(OrderNumber.THREE);

        for (int i = 0; i != m_OrderNumbers.Count; ++i)
        {;
            // 命令の追加
            //m_Orders.Add(m_OrderNumbers[i], m_OrdersOne);
            // 命令状態の追加
            m_OrderStatus.Add(m_OrderNumbers[i], OrderStatus.NULL);
        }

        // 命令の追加
        m_Orders.Add(OrderNumber.ONE, m_OrdersOne);
        m_Orders.Add(OrderNumber.TWO, m_OrdersTwo);
        m_Orders.Add(OrderNumber.THREE, m_OrdersThree);
        // 命令状態の追加
        //m_OrderStatus.Add(OrderNumber.ONE, OrderStatus.NULL);
        //m_OrderStatus.Add(OrderNumber.TWO, OrderStatus.NULL);
        //m_OrderStatus.Add(OrderNumber.THREE, OrderStatus.NULL);

        // 命令の追加
        for (int i = 0; i != m_OrderNumbers.Count; ++i)
        {
            // 命令番号の数だけ追加する
            for (int j = 0; j != m_OrderList.GetOrderStatus(m_OrderNumbers[i]).Length; ++j)
            {
                var orders = m_OrderList.GetOrders(m_OrderNumbers[i])[j];
                var state = m_OrderList.GetOrderStatus(m_OrderNumbers[i])[j];
                //m_Orders[m_OrderNumbers[i]].Add(m_OrderList.GetOrderStatus(m_OrderNumbers[i])[j], orders);
                m_Orders[m_OrderNumbers[i]].Add(state, orders);
                // 番号の設定
                //m_Orders[m_OrderNumbers[i]][m_OrderStatus[m_OrderNumbers[i]]].SetOrderNumber(m_OrderNumbers[i]);
                m_Orders[m_OrderNumbers[i]][state].SetOrderNumber(m_OrderNumbers[i]);
                m_Orders[m_OrderNumbers[i]][state].SetOrderState(state);
            }
            m_PrevOrders[m_OrderNumbers[i]] = OrderStatus.NULL;
        }

        // 命令変更格納リストに追加
        //m_ChangeOrders.Add((orders, numbers, count) => { Change(orders, numbers); });
        m_ChangeOrders.Add((number, dir, obj) => { m_Orders[number][m_OrderStatus[number]].StartAction(gameObject, obj); });
        m_ChangeOrders.Add((number, dir, obj) => { m_Orders[number][m_OrderStatus[number]].GetComponent<DirectionOrder>().StartAction(gameObject, dir); });
        // m_Orders[number][m_OrderStatus[number]].GetComponent<DirectionOrder>().StartAction(gameObject, dir);
    }

    // public virtual void ChangeOrder(OrderStatus order, OrderNumber number = OrderNumber.ONE)
    // 命令の変更を行います
    public virtual void ChangeOrder(OrderStatus order)
    {
        // 命令のあるオーダー番号の捜索
        OrderNumber number = OrderNumber.ONE;
        if (m_OrderList.IsOrder(OrderNumber.TWO, order)) number = OrderNumber.TWO;
        else if (m_OrderList.IsOrder(OrderNumber.THREE, order)) number = OrderNumber.THREE;
        // 変更
        Change(order, number, OrderDirection.NULL, 0);
    }

    // 命令の変更を行います(命令番号指定)
    public virtual void ChangeOrder(OrderStatus order, OrderNumber number = OrderNumber.ONE)
    {
        // 変更
        Change(order, number, OrderDirection.NULL, 0);
        //m_ChangeOrders[0](order, number, 0);
    }

    // public virtual void ChangeOrder(OrderStatus order, OrderDirection dir, OrderNumber number = OrderNumber.ONE)
    public virtual void ChangeOrder(OrderStatus order, OrderDirection dir)
    {
        // 命令のあるオーダー番号の捜索
        OrderNumber number = OrderNumber.ONE;
        if (m_OrderList.IsOrder(OrderNumber.TWO, order)) number = OrderNumber.TWO;
        else if (m_OrderList.IsOrder(OrderNumber.THREE, order)) number = OrderNumber.THREE;

        var orderDir = m_Orders[number][order].GetComponent<DirectionOrder>().GetDirection();
        // 命令がない場合は返す
        if (!CheckrOrder(order, number) || (m_OrderStatus[number] == order && orderDir == dir)) return;

        if(dir == OrderDirection.NULL) return;

        print("方向指定命令承認！");

        // 最後の行動
        m_Orders[number][m_OrderStatus[number]].EndAction(gameObject);

        // 命令状態の変更
        m_OrderStatus[number] = order;
        m_StateTimer = 0.0f;

        // 方向指定の最初の行動
        m_Orders[number][m_OrderStatus[number]].GetComponent<DirectionOrder>().StartAction(gameObject, dir);
    }

    public void Change(OrderStatus order, OrderNumber orderNum, OrderDirection dir, int number)
    {
        // 命令がない場合は返す
        if (!CheckrOrder(order, orderNum) || (m_OrderStatus[orderNum] == order)) return;
        print("命令承認！:" + orderNum.ToString() + ":" + m_OrderStatus[orderNum].ToString());
        // 最後の行動
        m_Orders[orderNum][m_OrderStatus[orderNum]].EndAction(gameObject);
        // 命令状態の変更
        m_OrderStatus[orderNum] = order;
        m_StateTimer = 0.0f;
        // 最初の行動
        m_ChangeOrders[number](orderNum, dir, m_ActionObject);
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

    // 命令方向の変更
    public void SetOrderDir(OrderDirection dir) { m_OrderDir = dir; }
    // 命令方向の取得
    public OrderDirection GetOrderDir() { return m_OrderDir; }
    #endregion

    #region イベント関数
    #region オーダーインターフェース
    // イベントでの呼び出し
    public void onOrder(OrderStatus order)
    {
        ChangeOrder(order);
    }

    // イベントでの呼び出し(方向指定)
    public void onOrder(OrderStatus order, OrderDirection dir)
    {
        ChangeOrder(order, dir);
    }
    // イベントでの呼び出し(停止処理)
    public void stopOrder()
    {
        // 全部の命令を停止命令に変更する
        for (int i = 0; i != m_OrderNumbers.Count; ++i)
        {
            m_Orders[m_OrderNumbers[i]][m_OrderStatus[m_OrderNumbers[i]]].StopAction(gameObject);
        }
    }
    // 
    public void stopOrder(OrderStatus order)
    {
        // 指定の命令を捜す
        for (int i = 0; i != m_OrderNumbers.Count; ++i)
        {
            // 命令がなければ、やり直す
            if (!m_Orders[m_OrderNumbers[i]].ContainsKey(order)) continue;
            // 指定した命令を停止させる
            m_Orders[m_OrderNumbers[i]][order].StopAction(gameObject);
        }
    }
    // イベントでの終了処理呼び出し
    public void endOrder(OrderNumber number)
    {
        ChangeOrder(OrderStatus.NULL, number);
    }
    // イベントでの参照オブジェクトの設定処理の呼び出し
    public void setObject(GameObject obj)
    {
        m_ActionObject = obj;
    }
    // イベントでの前回の命令の取得
    public OrderStatus getOrderState(OrderNumber number)
    {
        return m_PrevOrders[number];
    }
    #endregion

    #region ジェネラルインターフェース
    // ダメージ処理の呼び出し
    public void onDamage(int amount) { }
    //public void onShock() { }

    //public void onThrow() { }

    //public void onLift(GameObject obj) { }

    //public void onTakeDown() { }
    #endregion
    #endregion

    #region ジャミング関数
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
    #endregion

    #region 衝突判定関数
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
    #endregion
    #endregion

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
