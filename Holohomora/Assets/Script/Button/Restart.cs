using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    public Game_Manager gm;
    private ParticleSystem particule;

    public void Awake()
    {
        particule = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        particule.Play();
        gm.RestartGame();
    }
}
