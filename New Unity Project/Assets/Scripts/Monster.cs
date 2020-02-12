using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Monster : MonoBehaviour
{
    private bool moveToBattle;
    private bool battleReady;
    private float walkingSpeed = 10f;

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
    GameObject monsterHPSlider;

    private void LoadUI()
    {
        monsterHPSlider = this.transform.Find("Healthbar").Find("HealthSlider").gameObject;
        displayMonsterHP = this.transform.Find("Healthbar").Find("HealthText").GetComponent<TextMeshProUGUI>();
        monsterAnimator = this.GetComponent<Animator>();

        this.GetComponent<Button>().onClick.AddListener(() => takeDamage(GameData.sessionData.playerUpgrade.currentDamage));
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
            if (damage > 0 && currentHP - damage > 0)
            {
                currentHP -= damage;
                monsterAnimator.Play("damage");
                Vector3 tmp = monsterHPSlider.transform.localScale;
                tmp.x = currentHP/maxHP;
                monsterHPSlider.transform.localScale = tmp;
            }
            else if (currentHP - damage <= 0)
            {
                currentHP = 0;
                this.transform.Find("Healthbar").gameObject.SetActive(false);
                this.GetComponent<Button>().onClick.RemoveAllListeners();
                CancelInvoke();
                monsterAnimator.Play("death");
                StartCoroutine(FadeTo(0f, 1.0f));
                GameObject newCoin = Instantiate(Resources.Load("Prefabs/Coin") as GameObject, this.transform.position, this.transform.rotation, this.transform.parent);
                Invoke("Despawn", 1f);
            }
            displayMonsterHP.text = currentHP + "/" + maxHP;

            if (damage == GameData.sessionData.playerUpgrade.currentDamage)
            {
                FindObjectOfType<Player>().attack(); // Attack animation if it was dealt by player
            }
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
    //Fade Out
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            transform.GetComponent<Image>().color = newColor;
            yield return null;
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
