using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL        = 1 << 0,   // 命令なし
    MOVE        = 1 << 1,   // 移動
    STOP        = 1 << 2,   // 停止
    TURN_LEFT   = 1 << 3,   // 左回転
    TURN_RIGHT  = 1 << 4,   // 右回転
    LIFT        = 1 << 5,   // 持ち上げ
    PULL_OUT    = 1 << 6,   // 引き抜き
    TAKE_DOWN   = 1 << 7,   // 置く
    ATTACK      = 1 << 8,   // 攻撃
    ATTACK_HIGH = 1 << 9,   // 上攻撃
    ATTACK_LOW  = 1 << 10,  // 下攻撃
    PROTECT     = 1 << 11,  // 守備
    DESTRUCT    = 1 << 12,  // 自爆
    THROW       = 1 << 13,  // 投げる
}
