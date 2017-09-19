using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL        = 1 << 0,   // 命令なし
    MOVE        = 1 << 1,   // 移動
    STOP        = 1 << 2,   // 停止
    TURN_LEFT   = 1 << 3,   // 左回転
    TURN_RIGHT  = 1 << 4,   // 右回転
    LIFT        = 1 << 5,   // 持ち上げ
    TAKE_DOWN   = 1 << 6,   // 置く
    ATTACK      = 1 << 7,   // 攻撃
    ATTACK_HIGH = 1 << 8,   // 上攻撃
    ATTACK_LOW  = 1 << 9,   // 下攻撃
    PROTECT     = 1 << 10,  // 守備
    DESTRUCT    = 1 << 11,  // 自爆
    THROW       = 1 << 12,  // 投げる
}
