using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 右回転命令クラス
public class OrderTurnRight : Order {

    [SerializeField]
    private float m_TurnSpeed = 10.0f;  // 回転速度

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void UpdateAction(float deltaTime, GameObject obj)
    {
        print("Turn_Right");

        obj.transform.Rotate(obj.transform.up, m_TurnSpeed * deltaTime);
    }

    #region エディターのシリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(OrderTurnRight), true)]
    [CanEditMultipleObjects]
    public class OrderTurnRightEditor : Editor
    {
        SerializedProperty TurnSpeed;

        public void OnEnable()
        {
            TurnSpeed = serializedObject.FindProperty("m_TurnSpeed");
        }

        public override void OnInspectorGUI()
        {
            // 更新
            serializedObject.Update();

            // 自身の取得;
            OrderTurnRight order = target as OrderTurnRight;

            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇右回転の命令");

            // float
            TurnSpeed.floatValue = EditorGUILayout.FloatField("回転速度(m/s)", order.m_TurnSpeed);

            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion
}
