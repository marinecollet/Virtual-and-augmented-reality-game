using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    private MazeDoor mazeDoor;


    public AudioSource audioSource;

    void Start()
    {
        mazeDoor = gameObject.GetComponent<MazeDoor>();
    }

	public void openTheDoor()
    {
        MazeDoor otherDoor = mazeDoor.otherCell.GetEdge(mazeDoor.direction.GetOpposite()) as MazeDoor;
        if(otherDoor != null)
        {
            Animator otherAnim = otherDoor.gameObject.GetComponent<Animator>();
            if (otherAnim != null && otherAnim.GetBool("isOpen") != true)
            {
                //... the enemy should take damage.
                otherAnim.SetBool("isOpen", true);
            }

        }
        Animator anim = gameObject.GetComponent<Animator>();
        //If the EnemyHealth component exist...
        if (anim != null && anim.GetBool("isOpen") != true)
        {
            //... the enemy should take damage.
            anim.SetBool("isOpen", true);
        }

        audioSource.Play();
    }
}
