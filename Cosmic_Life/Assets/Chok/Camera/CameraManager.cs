using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log(Display.displays.Length);
        //Display.displays[1].Activate();
        PlayerPrefs.SetInt("UnitySelectMonitor", 1);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
