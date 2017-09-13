using System.Collections;
using UnityEngine;

public enum OrderStatus {
    NULL =          1 << 0,
    MOVE =          1 << 1,
    STOP =          1 << 2,
    TURN_LEFT =     1 << 3,
    TURN_RIGHT =    1 << 4,
    LIFT =          1 << 5,
    TAKE_DOWN =     1 << 6,
    ATTACK =        1 << 7,
    PROTECT =       1 << 8
}
