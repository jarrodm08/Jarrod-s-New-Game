﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool moveToBattle = false;
    private float runningSpeed = 2f;
    private float walkingSpeed = 1f;

    private Animator playerAnimator;
    private SessionData sessionData;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        sessionData = FindObjectOfType<SessionData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToBattle == true)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.right * Time.deltaTime * runningSpeed);
        }
    }

    public void attackMonster()
    {
        if (FindObjectOfType<Monster>().monsterCurrentHP - sessionData.playerDPS >= 0 | FindObjectOfType<Monster>().monsterCurrentHP < sessionData.playerDPS)
        {
            playerAnimator.Play("atkSlash", 0, 0);
            
            Invoke("monsterDamageDelay",0.40f); // Delay the damage animation
            if (FindObjectOfType<Monster>().monsterCurrentHP - sessionData.playerDPS > 0)
            {
                Invoke("attackMonster", 1f);
            }
        }
        else
        {
            Debug.Log("Tried to attack again even though the enemy has died");
        }
           
    }
    private void monsterDamageDelay()
    {
        FindObjectOfType<Monster>().takeDamage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "collider_Ground")
        {
            moveToBattle = true;
            playerAnimator.SetBool("isRunning",true);
        }
        if (collision.collider.tag == "collider_Battle")
        {
            moveToBattle = false;
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetBool("battleReady", true);
        }
    }
}