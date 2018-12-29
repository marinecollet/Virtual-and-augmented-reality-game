using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public Player player;
    public GameObject FirstRoom;
    ParticleSystem particle;

    bool playerIsIn;

    IEnumerator MovePlayerAfter()
    {

        yield return new WaitForSeconds(3);
        player.move(new Vector3(0f, 0f, 0f));
        Destroy(FirstRoom);
    }

    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        playerIsIn = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = true;
        }
        if (other.CompareTag("cheminette") && playerIsIn)
        {
            Destroy(other.gameObject);
            particle.Play();
            StartCoroutine(MovePlayerAfter());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = false;
        }
    }
}
