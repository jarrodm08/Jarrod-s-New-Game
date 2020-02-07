using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool moveToBattle = false;
    private float runningSpeed = 2f;

    private Animator playerAnimator;
    private AudioSource audioSrc;
    private AudioClip[] slashClips;

    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        audioSrc = this.GetComponent<AudioSource>();
        slashClips = new AudioClip[3];
        slashClips[0] = Resources.Load("Audio/slash1") as AudioClip;
        slashClips[1] = Resources.Load("Audio/slash2") as AudioClip;
        slashClips[2] = Resources.Load("Audio/slash3") as AudioClip;
    }

    void Update()
    {
        if (moveToBattle == true)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.right * Time.deltaTime * runningSpeed);
        }
    }

    private int audioIndex = 0;
    public void attack()
    {
        playerAnimator.Play("atkSlash", 0, 0);
        audioSrc.PlayOneShot(slashClips[audioIndex], GameData.sessionData.menuSettings.effectsVolume*10); // Times 10 because these audio clips are very quiet
        if (audioIndex <=1 )
        {
            audioIndex++;
        }
        else
        {
            audioIndex = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "collider_Ground")
        {
            moveToBattle = true;
            playerAnimator.SetBool("isRunning", true);
        }
        if (collision.collider.tag == "collider_Battle")
        {
            moveToBattle = false;
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("battleReady", true);
        }
    }
}
