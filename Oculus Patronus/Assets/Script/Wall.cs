using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shot") || other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
        }
    }
}
