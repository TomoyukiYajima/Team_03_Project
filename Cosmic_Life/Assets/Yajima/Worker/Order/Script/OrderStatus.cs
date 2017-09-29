using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL        = 1 << 0,   // 命令なし
    MOVE        = 1 << 1,   // 移動
    STOP        = 1 << 2,   // 停止
    RESUME      = 1 << 3,   // 再開
    TURN_LEFT   = 1 << 4,   // 左回転
    TURN_RIGHT  = 1 << 5,   // 右回転
    LIFT        = 1 << 6,   // 持ち上げ
    PULL_OUT    = 1 << 7,   // 引き抜き
    TAKE_DOWN   = 1 << 8,   // 置く
    ATTACK      = 1 << 9,   // 攻撃
    ATTACK_HIGH = 1 << 10,  // 上攻撃
    ATTACK_LOW  = 1 << 11,  // 下攻撃
    PROTECT     = 1 << 12,  // 守備
    DESTRUCT    = 1 << 13,  // 自爆
    THROW       = 1 << 14,  // 投げる
    ALLSTOP     = 1 << 15,  // 全停止
}
