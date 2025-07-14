using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BubbleScript : MonoBehaviour
{
    [SerializeField] int spawnChance = 10;
    [SerializeField] GameObject innerFish;
    public bool isBubbleColliding = false;
    public bool playWarning = false;
    [SerializeField] private Sprite[] commonFish;
    [SerializeField] private Sprite[] uncommonFish;
    [SerializeField] private Sprite[] rareFish;
    [SerializeField] private string[] commonFishNames;
    [SerializeField] private string[] uncommonFishNames;
    [SerializeField] private string[] rareFishNames;
    [SerializeField] private int commonChance, uncommonChance, rareChance;

    private int containedFish = -1;
    private string fishRarity = "null";
    private bool isFishInCatchZone = false;
    private bool isFishOutOfCatchZone = false;

    void Awake()
    {
        int fishAppearance = Random.Range(0, 100); // determines if fish spawns
        int fishChance = Random.Range(0, 100);
        if (fishAppearance < spawnChance) // chance of fish spawning (default is 10)
        {
            if (fishChance < rareChance) // rarest first to check others later
            {
                containedFish = Random.Range(0, rareFish.Length);
                innerFish.GetComponent<Image>().overrideSprite = rareFish[containedFish];
                innerFish.SetActive(true);
                fishRarity = "rare";
            }
            else if (fishChance < uncommonChance)
            {
                containedFish = Random.Range(0, uncommonFish.Length);
                innerFish.GetComponent<Image>().overrideSprite = uncommonFish[containedFish];
                innerFish.SetActive(true);
                fishRarity = "uncommon";
            }
            else
            {
                containedFish = Random.Range(0, commonFish.Length);
                innerFish.GetComponent<Image>().overrideSprite = commonFish[containedFish];
                innerFish.SetActive(true);
                fishRarity = "common";
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            SelfDestruct();
        }
        if (other.CompareTag("WarnZone") && containedFish >= 0)
        {
            playWarning = true;

        }
        if (other.CompareTag("CatchZone") && containedFish >= 0)
        {
            isFishInCatchZone = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CatchZone") && containedFish >= 0)
        {
            isFishInCatchZone = false;
            isFishOutOfCatchZone = true;
        }
    }

    public string NameGetter()
    {
        switch (fishRarity)
        {
            case "common":
                return commonFishNames[containedFish];
            case "uncommon":
                return uncommonFishNames[containedFish];
            case "rare":
                return rareFishNames[containedFish];
            default:
                return "Error: No fish";
        }
    }
    public bool GetFishState()
    {
        if (containedFish >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isInCatchZone()
    {
        return isFishInCatchZone;
    }
    public bool isOutOfCatchZone()
    {
        return isFishOutOfCatchZone;
    }
    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

}
