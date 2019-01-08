using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wand")|| other.CompareTag("Shot"))
        {
            Application.Quit();
        }
    }
}
