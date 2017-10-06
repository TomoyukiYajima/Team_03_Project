using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL            = 1 << 0,   // 命令なし
    MOVE            = 1 << 1,   // 移動
    STOP            = 1 << 2,   // 停止
    RESUME          = 1 << 3,   // 再開
    JUMP            = 1 << 4,   // ジャンプ
    TURN            = 1 << 5,   // 回転
    //TURN_LEFT       = 1 << 5,   // 左回転
    //TURN_RIGHT      = 1 << 6,   // 右回転
    LIFT            = 1 << 6,   // 持ち上げ
    PULL_OUT        = 1 << 7,   // 引き抜き
    TAKE_DOWN       = 1 << 8,   // 置く
    ATTACK          = 1 << 9,   // 攻撃
    ATTACK_HIGH     = 1 << 10,  // 上攻撃
    ATTACK_LOW      = 1 << 11,  // 下攻撃
    ATTACK_MOW_DOWN = 1 << 12,  // 薙ぎ払い
    PROTECT         = 1 << 13,  // 守備
    DESTRUCT        = 1 << 14,  // 自爆
    THROW           = 1 << 15,  // 投げる
    ALLSTOP         = 1 << 16,  // 全停止
}

public enum AttackOrderStatus
{
    NORMAL      = 1 << 0,   // 攻撃
    HIGH        = 1 << 1,   // 上攻撃
    LOW         = 1 << 2,   // 下攻撃
    MOW_DOWN    = 1 << 3,   // 薙ぎ払い
}
