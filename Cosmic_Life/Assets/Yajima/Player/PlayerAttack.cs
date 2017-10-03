using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            ExecuteEvents.Execute<IGeneralEvent>(
                other.gameObject,
                null,
                (receiveTarget, y) => receiveTarget.onDamage(10));
        }
    }


}
