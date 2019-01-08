using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    public Game_Manager gm;

    private void OnTriggerEnter(Collider other)
    {
        gm.RestartGame();
    }
}
