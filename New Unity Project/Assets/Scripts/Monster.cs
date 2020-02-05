using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Monster : MonoBehaviour
{
    private bool moveToBattle = false;
    private float walkingSpeed = 1f;
    private Animator monsterAnimator;

    private GameObject monsterHPSlider;
    private TextMeshProUGUI displayMonsterHP;
    private SessionData sessionData;
    public float monsterMaxHP;
    public float monsterCurrentHP;

    void Start()
    {
        sessionData = FindObjectOfType<SessionData>();
        monsterAnimator = this.GetComponent<Animator>();
        monsterHPSlider = this.transform.Find("Healthbar").Find("HealthSlider").gameObject;
        displayMonsterHP = this.transform.Find("Healthbar").Find("HealthText").GetComponent<TextMeshProUGUI>();

    }


    void Update()
    {
        if (moveToBattle == true)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position - transform.right * Time.deltaTime * walkingSpeed);
        }

        //update healthbar
        if (monsterCurrentHP != null && monsterMaxHP != 0f)
        {
            displayMonsterHP.text = monsterCurrentHP + "/" + monsterMaxHP;
            Vector3 temp = monsterHPSlider.transform.localScale;
            temp.x = monsterCurrentHP / monsterMaxHP;
            monsterHPSlider.transform.localScale = temp;
        }
    }

    public void takeDamage()
    {
        if (monsterCurrentHP - sessionData.playerDPS >= 1)
        {
            monsterCurrentHP -= sessionData.playerDPS;
            monsterAnimator.Play("damage", 0, 0);

        }
        else
        {
            monsterCurrentHP = 0;
            monsterAnimator.Play("death", 0, 0);
            this.GetComponent<Image>().CrossFadeAlpha(0f,1f,false);
            Invoke("monsterDeath",1f);
        }
    }

    private void monsterDeath()
    {
        Destroy(this.gameObject);
        FindObjectOfType<GameManager>().monsterDied(this.transform);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "collider_Ground")
        {
            moveToBattle = true;
            monsterAnimator.SetBool("isWalking", true);
        }
        if (collision.collider.tag == "collider_Battle")
        {
            moveToBattle = false;
            monsterAnimator.SetBool("isWalking", false);
            FindObjectOfType<Player>().attackMonster();
        }
    }
}
