using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        loadClouds();
        //GameObject newCloud = Instantiate(cloudPrefab,cloudSpawnA.transform.position,cloudSpawnA.transform.rotation,cloudSpawnA.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private GameObject cloudPrefab;
    private GameObject spawnA;
    private GameObject spawnB;
    Dictionary<Sprite, GameObject> cloudDic = new Dictionary<Sprite, GameObject>();
    Dictionary<int, Dictionary<Sprite, GameObject>> cloudSpawnsDic = new Dictionary<int, Dictionary<Sprite, GameObject>>();
    //Dictionary<Sprite, GameObject> cloudDic = new Dictionary<Sprite, GameObject>();

    private void loadClouds()
    {
        Sprite[] cloudSprites = Resources.LoadAll<Sprite>("Art/Main Menu/Clouds"); // Load cloud sprites
        spawnA = this.transform.parent.Find("CloudSpawnA").Find("Spawn").gameObject;
        spawnB = this.transform.parent.Find("CloudSpawnB").Find("Spawn").gameObject;
        cloudPrefab = Resources.Load<GameObject>("Art/Main Menu/Cloud");



        cloudDic.Add(cloudSprites[0],spawnB);
        cloudDic.Add(cloudSprites[1],spawnA);
        cloudDic.Add(cloudSprites[3],spawnB);
        cloudDic.Add(cloudSprites[2],spawnA);

        int cloudNum = 0;
        foreach (KeyValuePair<Sprite,GameObject> p in cloudDic)
        {
            cloudNum++;
            cloudSpawnsDic.Add(cloudNum,p);
        }

        spawnCloud();
    }

    private int cloudNum = 0;

    public void spawnCloud()
    {
        foreach (KeyValuePair<int,Dictionary<Sprite,GameObject>> p in cloudSpawnsDic)
        {
            Debug.Log("Cloud #"+ p.Key + " has sprite: " + p.Value.);
        }
    }
}
