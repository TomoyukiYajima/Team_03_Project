using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL            = 1 << 0,   // 命令なし
    MOVE            = 1 << 1,   // 移動
    STOP            = 1 << 2,   // 停止
    RESUME          = 1 << 3,   // 再開
    JUMP            = 1 << 4,   // ジャンプ
    TURN            = 1 << 5,   // 回転
    LIFT            = 1 << 6,   // 持つ
    LIFT_UP         = 1 << 7,   // 持ち上げ
    PULL_OUT        = 1 << 8,   // 引き抜き
    TAKE_DOWN       = 1 << 9,   // 置く
    ATTACK          = 1 << 10,  // 攻撃
    ATTACK_HIGH     = 1 << 11,  // 上攻撃
    ATTACK_LOW      = 1 << 12,  // 下攻撃
    ATTACK_MOW_DOWN = 1 << 13,  // 薙ぎ払い
    PROTECT         = 1 << 14,  // 守備
    DESTRUCT        = 1 << 15,  // 自爆
    THROW           = 1 << 16,  // 投げる
    ALLSTOP         = 1 << 17,  // 全停止
    LOOK            = 1 << 18   // 向く
}

// 移動命令状態
public enum MoveOrderStatus
{
    NULL    = 1 << 0,   // 命令なし
    MOVE    = 1 << 1,   // 移動
    STOP    = 1 << 2,   // 停止
    TURN    = 1 << 3,   // 回転
    LOOK    = 1 << 4    // 向く
}

// 攻撃命令状態
public enum AttackOrderStatus
{
    NULL        = 1 << 0,   // 命令なし
    NORMAL      = 1 << 1,   // 攻撃
    HIGH        = 1 << 2,   // 上攻撃
    LOW         = 1 << 3,   // 下攻撃
    MOW_DOWN    = 1 << 4,   // 薙ぎ払い
}

// 持ち命令状態
public enum LiftOrderStatus
{
    NULL        = 1 << 0,   // 命令なし
    LIFT        = 1 << 1,   // 持ち
    LIFT_UP     = 1 << 2,   // 持ち上げ
    PULL_OUT    = 1 << 3,   // 引き抜き
    TAKE_DOWN   = 1 << 4,   // 置き
    THROW       = 1 << 5    // 投げ状態
}
