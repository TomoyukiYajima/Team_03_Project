using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OrderList : MonoBehaviour {

    // 命令状態リスト
    [SerializeField]
    private OrderStatus[] m_OrderStatus;
    // 命令リスト
    [SerializeField]
    private Order[] m_Orders;

    // 命令状態リストの取得
    public OrderStatus[] GetOrderStatus() { return m_OrderStatus; }

    // 命令リストの取得
    public Order[] GetOrders() { return m_Orders; }

    #region エディターのシリアライズ変更
    // 変数名を日本語に変換する機能
    // CustomEditor(typeof(Enemy), true)
    // 継承したいクラス, trueにすることで、子オブジェクトにも反映される
#if UNITY_EDITOR
    [CustomEditor(typeof(OrderList), true)]
    [CanEditMultipleObjects]
    public class OrderListEditor : Editor
    {
        SerializedProperty OrderStatus;
        SerializedProperty Orders;

        public void OnEnable()
        {
            OrderStatus = serializedObject.FindProperty("m_OrderStatus");
            Orders = serializedObject.FindProperty("m_Orders");
        }

        public override void OnInspectorGUI()
        {
            // 更新
            serializedObject.Update();

            // 自身の取得;
            OrderList orders = target as OrderList;

            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇命令リスト");

            // 配列
            EditorGUILayout.PropertyField(OrderStatus, new GUIContent("命令"), true);
            //EditorGUILayout.PropertyField(MovePoints, new GUIContent("徘徊ポイント"), true);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("〇実行する命令");
            EditorGUILayout.PropertyField(Orders, new GUIContent("実行"), true);

            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion
}
