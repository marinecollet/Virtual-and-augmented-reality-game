using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch))
        {
            Debug.Log("Button One pressed");
        }
        if (OVRInput.GetDown(OVRInput.Touch.Two, OVRInput.Controller.Touch))
        {
            Debug.Log("Button two pressed");
        }
    }
}
