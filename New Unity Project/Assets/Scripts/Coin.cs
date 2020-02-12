using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float coinValue;
    private float speed = 5f;
    public Transform target;
    
    
    // Start is called before the first frame update
    void Start()
    {
        coinValue = Mathf.Ceil(FindObjectOfType<Monster>().maxHP * 0.008f + 0.0002f * Mathf.Min(GameData.sessionData.playerData.stage, 150));
        target = Camera.main.transform.Find("Canvas").Find("MainUI").Find("GoldDisplay").Find("Icon");
    }

    private bool bankDB = false;
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the coin and bankIcon are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            if (bankDB == false)
            {
                bankDB = true;
                GameData.sessionData.playerData.gold += coinValue;
                Destroy(this.gameObject);
            }
        }
    }
}
