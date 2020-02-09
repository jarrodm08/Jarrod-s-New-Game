﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Monster : MonoBehaviour
{
    private bool moveToBattle;
    private bool battleReady;
    private float walkingSpeed = 1f;

    public float maxHP;
    public float currentHP;
    private Animator monsterAnimator;

    void Start()
    {
        maxHP = Mathf.Round(17.5f * Mathf.Pow(1.39f, Mathf.Min(GameData.sessionData.playerData.stage, 115)) * Mathf.Pow(1.13f, Mathf.Max(GameData.sessionData.playerData.stage - 115, 0)));
        currentHP = maxHP;
        LoadUI();  
    }


    void Update()
    {
        if (moveToBattle == true)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position - transform.right * Time.deltaTime * walkingSpeed);
        }

    }

    TextMeshProUGUI displayMonsterHP;
    private void LoadUI()
    {
        GameObject monsterHPSlider = this.transform.Find("Healthbar").Find("HealthSlider").gameObject;
        displayMonsterHP = this.transform.Find("Healthbar").Find("HealthText").GetComponent<TextMeshProUGUI>();
        monsterAnimator = this.GetComponent<Animator>();

        this.GetComponent<Button>().onClick.AddListener(() => takeDamage(GameData.sessionData.playerData.tapDamage));
        displayMonsterHP.text = currentHP + "/" + maxHP;

        InvokeRepeating("HeroDamageRelay", 1,1);
        
        
    }
    // use this as a relay because we cannot invoke repeating if the method needs params
    private void HeroDamageRelay()
    {
        takeDamage(UpgradeUtils.GetTotalHeroDPS());
    }

    public void takeDamage(float damage)
    {
        if (battleReady == true)
        {
            if (currentHP - damage > 0)
            {
                currentHP -= damage;
                monsterAnimator.Play("damage", 0, 0);
            }
            else if (currentHP - damage <= 0)
            {
                currentHP = 0;
                this.transform.Find("Healthbar").gameObject.SetActive(false);
                this.GetComponent<Button>().onClick.RemoveAllListeners();
                CancelInvoke();
                monsterAnimator.Play("death", 0, 0);
                this.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
                GameObject newCoin = Instantiate(Resources.Load("Prefabs/Coin") as GameObject, this.transform.position, this.transform.rotation, this.transform.parent);
                Invoke("Despawn", 1f);
            }
            displayMonsterHP.text = currentHP + "/" + maxHP;

            if (damage == GameData.sessionData.playerData.tapDamage)
            {
                FindObjectOfType<Player>().attack(); // Attack animation if it was dealt by player
            }
        }
    }
    public void takeHeroDamage()
    {
        if (battleReady == true && UpgradeUtils.GetTotalHeroDPS() > 0)
        {
            if (currentHP - UpgradeUtils.GetTotalHeroDPS() > 0)
            {
                currentHP -= UpgradeUtils.GetTotalHeroDPS();
                monsterAnimator.Play("damage", 0, 0);
            }
            else if (currentHP - UpgradeUtils.GetTotalHeroDPS() <= 0)
            {
                currentHP = 0;
                this.transform.Find("Healthbar").gameObject.SetActive(false);
                this.GetComponent<Button>().onClick.RemoveAllListeners();
                monsterAnimator.Play("death", 0, 0);
                this.GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
                GameObject newCoin = Instantiate(Resources.Load("Prefabs/Coin") as GameObject, this.transform.position, this.transform.rotation, this.transform.parent);
                Invoke("Despawn", 1f);
            }
            displayMonsterHP.text = currentHP + "/" + maxHP;
        }
    }

    private void Despawn()
    {
        Destroy(this.gameObject);
        if (GameData.sessionData.playerData.monsterNum <= 9)
        {
            GameData.sessionData.playerData.monsterNum += 1;
            FindObjectOfType<GameManager>().SpawnMonster(this.transform);
        }
        else
        {
            GameData.sessionData.playerData.stage += 1;
            GameData.sessionData.playerData.monsterNum = 1;
            FindObjectOfType<GameManager>().SpawnMonster();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "collider_Ground" && moveToBattle == false && battleReady == false)
        {
            moveToBattle = true;
            monsterAnimator.SetBool("isWalking",true);
        }
        if (collision.collider.tag == "collider_Battle")
        {
            moveToBattle = false;
            battleReady = true;
            monsterAnimator.SetBool("isWalking", false);
        }

    }
}
