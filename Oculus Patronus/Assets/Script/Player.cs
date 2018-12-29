using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
    public int lifeAtStart;
    public int lifeLosePerShot;
    public bool isDead { get; private set; }
    public Image damageOverlay;
    public Transform wand;
    public Transform spellDetector;
    public int life;
    public Transform localAvatar;
    public Transform FirstRoom;

    private bool isHurt = false;
    private float fadeOutTime;

    private IEnumerator MoveToFirstRoomAfter()
    {
        yield return new WaitForSeconds(3);
        FirstRoom.gameObject.SetActive(true);
        this.move(new Vector3(28,0,0));
        life = lifeAtStart;
        isDead = false;
    }

    public void Awake()
    {
        isDead = false;
        life = lifeAtStart;
        damageOverlay.canvasRenderer.SetAlpha(0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
            this.life -= lifeLosePerShot;

            isHurt = true;

            damageOverlay.canvasRenderer.SetAlpha((lifeAtStart - life)/ lifeAtStart);
            Debug.Log("a "+ (float)((lifeAtStart - life) / lifeAtStart));
            fadeOutTime = lifeAtStart - life;
            if (life <= 0)
            {
                isDead = true;
                Destroy(wand.gameObject);
                Destroy(spellDetector.gameObject);
                StartCoroutine(MoveToFirstRoomAfter());
            }
        }
    }

    private void Update()
    {
        if(isHurt && !isDead)
        {
            fadeOutTime -= Time.deltaTime;
            if (fadeOutTime < 0)
            {
                fadeOutTime = 0;
                isHurt = false;
            }
            damageOverlay.canvasRenderer.SetAlpha(fadeOutTime / lifeAtStart);
        }
    }

    public void reset()
    {
        life = lifeAtStart;
    }

    public void move(Vector3 newPosition){
        this.transform.localPosition = newPosition;
        localAvatar.localPosition = newPosition;
    }

}