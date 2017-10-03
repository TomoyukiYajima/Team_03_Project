using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OrderList : MonoBehaviour {

    // 命令状態リスト1
    [SerializeField]
    private OrderStatus[] m_OrderOneStatus;
    // 命令状態リスト2
    [SerializeField]
    private OrderStatus[] m_OrderTwoStatus;
    // 命令状態リスト3
    [SerializeField]
    private OrderStatus[] m_OrderThreeStatus;
    // 命令状態リスト
    private Dictionary<OrderNumber, OrderStatus[]> m_OrderStatus =
        new Dictionary<OrderNumber, OrderStatus[]>();
    // 命令リスト1
    [SerializeField]
    private Order[] m_OrdersOne;
    // 命令リスト2 
    [SerializeField]
    private Order[] m_OrdersTwo;
    // 命令リスト3
    [SerializeField]
    private Order[] m_OrdersThree;
    // 命令リスト
    private Dictionary<OrderNumber, Order[]> m_Orders =
        new Dictionary<OrderNumber, Order[]>();

    // Use this for initialization
    //void Start()
    //{
    //    //// 命令状態リストの追加
    //    //m_OrderStatus.Add(OrderNumber.ONE, m_OrderOneStatus);
    //    //m_OrderStatus.Add(OrderNumber.TWO, m_OrderTwoStatus);
    //    //m_OrderStatus.Add(OrderNumber.THREE, m_OrderThreeStatus);
    //    //// 命令リストの追加
    //    //m_Orders.Add(OrderNumber.ONE, m_OrdersOne);
    //    //m_Orders.Add(OrderNumber.TWO, m_OrdersTwo);
    //    //m_Orders.Add(OrderNumber.THREE, m_OrdersThree);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //}

    // 命令の初期化
    public void InitializeOrder()
    {
        // 命令状態リストの追加
        m_OrderStatus.Add(OrderNumber.ONE, m_OrderOneStatus);
        m_OrderStatus.Add(OrderNumber.TWO, m_OrderTwoStatus);
        m_OrderStatus.Add(OrderNumber.THREE, m_OrderThreeStatus);
        // 命令リストの追加
        m_Orders.Add(OrderNumber.ONE, m_OrdersOne);
        m_Orders.Add(OrderNumber.TWO, m_OrdersTwo);
        m_Orders.Add(OrderNumber.THREE, m_OrdersThree);
    }

    // 命令状態リストの取得
    //public OrderStatus[] GetOrderStatus() { return m_OrderOneStatus; }
    public OrderStatus[] GetOrderStatus(OrderNumber number) { return m_OrderStatus[number]; }

    // 命令リストの取得
    public Order[] GetOrders(OrderNumber number) { return m_Orders[number]; }

    // 指定の命令があるかを返します
    public bool IsOrder(OrderNumber number, OrderStatus status)
    {
        //int state = 0;

        // 加算
        for(int i = 0; i != m_OrderStatus[number].Length; ++i)
        {
            if (m_OrderStatus[number][i] == status) return true;
            //state += (int)m_OrderStatus[number][i];
        }
        // 命令がない
        return false;
    }

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(OrderList), true)]
    [CanEditMultipleObjects]
    public class OrderListEditor : Editor
    {
        SerializedProperty OrderOneStatus;
        SerializedProperty OrderTwoStatus;
        SerializedProperty OrderThreeStatus;
        SerializedProperty OrdersOne;
        SerializedProperty OrdersTwo;
        SerializedProperty OrdersThree;

        public void OnEnable()
        {
            OrderOneStatus = serializedObject.FindProperty("m_OrderOneStatus");
            OrderTwoStatus = serializedObject.FindProperty("m_OrderTwoStatus");
            OrderThreeStatus = serializedObject.FindProperty("m_OrderThreeStatus");
            OrdersOne = serializedObject.FindProperty("m_OrdersOne");
            OrdersTwo = serializedObject.FindProperty("m_OrdersTwo");
            OrdersThree = serializedObject.FindProperty("m_OrdersThree");
        }

        public override void OnInspectorGUI()
        {
            // 更新
            serializedObject.Update();

            // 自身の取得;
            OrderList orders = target as OrderList;

            EditorGUILayout.Space();
            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇命令リスト1");
            // 命令リスト1
            EditorGUILayout.PropertyField(OrderOneStatus, new GUIContent("命令リスト"), true);
            //EditorGUILayout.Space();
            EditorGUILayout.PropertyField(OrdersOne, new GUIContent("実行リスト"), true);

            EditorGUILayout.Space();
            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇命令リスト2");
            // 命令リスト1
            EditorGUILayout.PropertyField(OrderTwoStatus, new GUIContent("命令リスト"), true);
            //EditorGUILayout.Space();
            EditorGUILayout.PropertyField(OrdersTwo, new GUIContent("実行リスト"), true);

            EditorGUILayout.Space();
            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇命令リスト3");
            // 命令リスト1
            EditorGUILayout.PropertyField(OrderThreeStatus, new GUIContent("命令リスト"), true);
            //EditorGUILayout.Space();
            EditorGUILayout.PropertyField(OrdersThree, new GUIContent("実行リスト"), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion
}
