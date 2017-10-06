using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OrderTurn : DirectionOrder {

    [SerializeField]
    private float m_TurnSpeed = 10.0f;  // 回転速度
    private int m_Direction = 1;        // 回転方向

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    public override void StartAction(GameObject obj)
    {
        base.StartAction(obj);
    }

    public override void Action(float deltaTime, GameObject obj)
    {
        print("Turn");

        obj.transform.Rotate(obj.transform.up, m_TurnSpeed * m_Direction * deltaTime);
    }

    protected override void SetDirection(GameObject obj)
    {
        switch (m_OrderDirection)
        {
            case OrderDirection.RIGHT: m_Direction = 1; break;
            case OrderDirection.LEFT: m_Direction = -1; break;
        }
    }

    #region エディターのシリアライズ変更
#if UNITY_EDITOR
    [CustomEditor(typeof(OrderTurnLeft), true)]
    [CanEditMultipleObjects]
    public class OrderTurnLeftEditor : Editor
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
            OrderTurn order = target as OrderTurn;

            // エディタ上でのラベル表示
            EditorGUILayout.LabelField("〇回転の命令");

            // float
            TurnSpeed.floatValue = EditorGUILayout.FloatField("回転速度(m/s)", order.m_TurnSpeed);

            // Unity画面での変更を更新する(これがないとUnity画面で変更が表示されない)
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    #endregion
}
