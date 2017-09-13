using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 入力状態
public enum InputState
{
    INPUT_OK = 1 << 0,
    INPUT_CANCEL = 1 << 1,
    INPUT_X = 1 << 2,
    INPUT_Y = 1 << 3,
    INPUT_START = 1 << 4,
    INPUT_SELECT = 1 << 5,
    INPUT_TRIGGER_LEFT = 1 << 6,
    INPUT_TRIGGER_RIGHT = 1 << 7,
}

//// キー入力状態
//public enum InputKeyState
//{
//    KEY_SPACE = 1 << 0,
//    KEY_Z = 1 << 1,
//    KEY_A = 1 << 2,
//    KEY_D = 1 << 3,
//    KEY_Q = 1 << 4,
//    KEY_E = 1 << 5,
//    KEY_P = 1 << 6,
//    KEY_I = 1 << 7,
//}

//// ゲームパッド入力状態
//public enum InputGamePadState
//{
//    GAMEPAD_B = 1 << 0,
//    GAMEPAD_A = 1 << 1,
//    GAMEPAD_X = 1 << 2,
//    GAMEPAD_Y = 1 << 3,
//    GAMEPAD_START = 1 << 4,
//    GAMEPAD_SELECT = 1 << 5,
//    GAMEPAD_TRIGGER_LEFT = 1 << 6,
//    GAMEPAD_TRIGGER_RIGHT = 1 << 7,
//}
