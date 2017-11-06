using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus
{
    //巡回
    RoundState,
    //巡回しない
    NonRoundState,
    //追跡中
    Chasing,
    //追跡中(見失っている)
    ChasingButLosed,
    //攻撃
    Attack,
    //元の位置に戻る状態
    ReturnPosition,

    None
}
