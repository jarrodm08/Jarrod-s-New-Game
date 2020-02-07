using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float coinValue;
    public Transform target;
    private float speed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool bankDB = false;
    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            if (bankDB == false)
            {
                bankDB = true;
                //FindObjectOfType<SessionData>().playerGold += coinValue;
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "collider_Bank")
        {
            Debug.Log("BANK IT");
              
        }
    }
}
