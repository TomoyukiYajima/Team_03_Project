using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerInputManager.GetInputDown(InputState.INPUT_OK)) print("OK");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_CANCEL)) print("Cancel");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_X)) print("X");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_Y)) print("Y");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_START)) print("Start");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_SELECT)) print("Select");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_LEFT)) print("Trigger_Left");
        if (PlayerInputManager.GetInputDown(InputState.INPUT_TRIGGER_RIGHT)) print("Trigger_Right");

        //print(PlayerInputManager.GetHorizontal().ToString());
        //print(PlayerInputManager.GetVertical().ToString());
    }
}
