using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Fire : MonoBehaviour
{

    public Player player;

    public GameObject FirstRoom;
    ParticleSystem particle;

    bool playerIsIn;

    /** changement de musique **/
    public AudioMixerSnapshot in_game;

    IEnumerator MovePlayerAfter()
    {

        yield return new WaitForSeconds(3);
        if(player is PlayerSansCasque)
        {
            player.move(new Vector3(0.5f, 0, 0.5f));
            player.spellDetector.gameObject.SetActive(true);
        }
        else
            player.move(new Vector3(0.5f, 0f, 0.5f));
        FirstRoom.SetActive(false);

        in_game.TransitionTo(60 / 128);
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
