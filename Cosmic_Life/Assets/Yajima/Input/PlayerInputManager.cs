using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    #region 変数
    //// 状態関数テーブル
    //protected Dictionary<AnimalState, Func<float, int>> m_Status =
    //    new Dictionary<AnimalState, Func<float, int>>();
    // 入力状態
    // (呼び出すキー, Func(呼び出す関数の引数, 戻り値))
    private static Dictionary<InputState, Func<InputState, bool>> m_InputStatus =
        new Dictionary<InputState, Func<InputState, bool>>();

    // キー入力状態
    //private static Dictionary<InputKeyState, Func<bool>> m_InputKeys =
    //    new Dictionary<InputKeyState, Func<bool>>();
    //private static Dictionary<InputState, Func<bool>> m_InputKeys =
    //    new Dictionary<InputState, Func<bool>>();
    private static Dictionary<InputState, KeyCode> m_InputKeys =
        new Dictionary<InputState, KeyCode>();

    // ゲームパッド入力状態
    private static Dictionary<InputState, string> m_InputGamePad =
        new Dictionary<InputState, string>();
    #endregion

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update () {}

    #region private関数
    // 入力直後か？
    private static bool InputDown(InputState state)
    {
        return Input.GetKeyDown(m_InputKeys[state]) | Input.GetButtonDown(m_InputGamePad[state]);
        //return m_InputKeys[state]();
    }
    // 入力中か？
    private static bool InputStay(InputState state)
    {
        return Input.GetKey(m_InputKeys[state]) | Input.GetButton(m_InputGamePad[state]);
    }
    // 入力後か？
    private static bool InputUp(InputState state)
    {
        return Input.GetKeyUp(m_InputKeys[state]) | Input.GetButtonUp(m_InputGamePad[state]);
    }
    #endregion

    // 入力直後か？
    public static bool GetInputDown(InputState state)
    {
        if(m_InputKeys.Count == 0) InitializeInput();

        //return m_InputStatus[state](state);
        return InputDown(state);
    }

    // 入力中か？
    public static bool GetInputStay(InputState state)
    {
        if (m_InputKeys.Count == 0) InitializeInput();

        return InputStay(state);
    }

    // 入力後か？
    public static bool GetInputUp(InputState state)
    {
        if (m_InputKeys.Count == 0) InitializeInput();

        return InputUp(state);
    }

    // X軸の入力値の取得
    public static float GetHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    // Y軸の入力値の取得
    public static float GetVertical()
    {
        return Input.GetAxis("Vertical");
    }

    // 入力の初期化
    public static void InitializeInput()
    {
        // キー入力状態の追加
        //m_InputKeys.Add(InputState.INPUT_OK, () => { return Input.GetKeyDown(KeyCode.Space); });
        m_InputKeys.Add(InputState.INPUT_OK, KeyCode.Space);
        m_InputKeys.Add(InputState.INPUT_CANCEL, KeyCode.B);
        m_InputKeys.Add(InputState.INPUT_X, KeyCode.Z);
        m_InputKeys.Add(InputState.INPUT_Y, KeyCode.C);
        m_InputKeys.Add(InputState.INPUT_START, KeyCode.Q);
        m_InputKeys.Add(InputState.INPUT_SELECT, KeyCode.E);
        m_InputKeys.Add(InputState.INPUT_TRIGGER_LEFT, KeyCode.A);
        m_InputKeys.Add(InputState.INPUT_TRIGGER_RIGHT, KeyCode.D);

        // ゲームパッド入力状態の追加
        m_InputGamePad.Add(InputState.INPUT_OK, "OK");
        m_InputGamePad.Add(InputState.INPUT_CANCEL, "Cancel");
        m_InputGamePad.Add(InputState.INPUT_X, "X");
        m_InputGamePad.Add(InputState.INPUT_Y, "Y");
        m_InputGamePad.Add(InputState.INPUT_START, "Start");
        m_InputGamePad.Add(InputState.INPUT_SELECT, "Select");
        m_InputGamePad.Add(InputState.INPUT_TRIGGER_LEFT, "Triggrt_Left");
        m_InputGamePad.Add(InputState.INPUT_TRIGGER_RIGHT, "Triggrt_Right");

        // 入力状態の追加
        //m_InputStatus.Add(InputState.INPUT_A, (input) => { return Input.GetKeyDown(KeyCode.Space); });

        //m_InputStatus.Add(InputState.INPUT_OK, (input) => { return m_InputKeys[InputKeyState.KEY_SPACE](); });


        //m_InputStatus.Add(InputState.INPUT_OK, (input) => { return InputDown(input); });
        //m_InputStatus.Add(InputState.INPUT_CANCEL, (input) => { return InputDown(input); });
    }
}
