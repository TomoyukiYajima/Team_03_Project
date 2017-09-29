using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IGeneralEvent
{
    public void onDamage(int amount)
    {
        Destroy(this.gameObject);
    }
}
