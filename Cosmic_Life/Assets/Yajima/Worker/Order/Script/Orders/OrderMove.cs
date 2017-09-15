using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 移動命令クラス
public class OrderMove : Order {

    [SerializeField]
    private float m_MoveSpeed = 3.0f;  // 移動速度

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Move");

        obj.transform.position += obj.transform.forward * m_MoveSpeed * deltaTime;
    }

    #region エディターのシリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(OrderMove), true)]
    [CanEditMultipleObjects]
    public class OrderMoveEditor : Editor
    {
        SerializedProperty MoveSpeed;

        public void OnEnable()
        {
            MoveSpeed = serializedObject.FindProperty("m_MoveSpeed");
        }

        public override void OnInspectorGUI()
        {
            // 更新
            serializedObject.Update();

            // 自身の取得;
            OrderMove order = target as OrderMove;

            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇移動の命令");

            // float
            MoveSpeed.floatValue = EditorGUILayout.FloatField("移動速度(m/s)", order.m_MoveSpeed);

            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion
}
