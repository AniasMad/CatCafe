using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FishGameActivate : MonoBehaviour
{
    [Header("System Variables!")]
    [SerializeField] private GameObject player;
    [SerializeField] private float river_distance = 4.0f;
    [SerializeField] private GameObject display_text;
    [SerializeField] private Transform canvas;
    private GameObject river_start, river_end;
    public int meow = 0; // debug int
    private bool fishingStarts = false;
    private bool fishingInProgress = false;
    private bool fishingEnding = false;
    // Fishing Game Variables
    [Header("Fishing Game stuff~")]
    [SerializeField] private float bubbleSpeed = 100.0f;
    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;
    [SerializeField] private GameObject catchZone;
    [SerializeField] private float spawnTime = 3.0f;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject bubblezone;
    private float timeToSpawn = 0f;
    private int whichSpawnPoint = 0;
    private GameObject[] bubblesArray; // array of bubble objects
    private int bubblesIndex = 0; // which bubble is created on the screen
    private int howManyBubbles = 0; // defines how many bubbles are currently in the array

    void Start()
    {
        river_start = this.transform.Find("river_start").gameObject;
        river_end = this.transform.Find("river_end").gameObject;

        bubblesArray = new GameObject[6];
        bubblezone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fishingStarts || fishingInProgress)
        {
            if (fishingStarts)
            {
                fishingInProgress = true;
                fishingStarts = false;
                bubblezone.SetActive(true);
            }
            if (timeToSpawn <= 0 && howManyBubbles < 6)
            {
                switch (whichSpawnPoint)
                {
                    case 1:
                        if (bubblesArray[bubblesIndex] == null)
                        {
                            bubblesArray[bubblesIndex] = Instantiate(bubble, spawn1.position, Quaternion.identity, canvas);
                            whichSpawnPoint = 2;
                        }
                        break;
                    case 2:
                        if (bubblesArray[bubblesIndex] == null)
                        {
                            bubblesArray[bubblesIndex] = Instantiate(bubble, spawn2.position, Quaternion.identity, canvas);
                            whichSpawnPoint = 1;
                        }
                        break;
                    default:
                        if (bubblesArray[bubblesIndex] == null)
                        {
                            bubblesArray[bubblesIndex] = Instantiate(bubble, spawn1.position, Quaternion.identity, canvas);
                            whichSpawnPoint = 2;
                        }
                        break;
                }
                howManyBubbles += 1;
                timeToSpawn += spawnTime;
                bubblesIndex++;
            }
            for (int i = 0; i < howManyBubbles; i++)
            {
                if (bubblesArray[i] != null)
                {
                    bubblesArray[i].transform.localPosition = new Vector2(
                    bubblesArray[i].GetComponent<RectTransform>().localPosition.x - (bubbleSpeed * Time.deltaTime),
                    bubblesArray[i].GetComponent<RectTransform>().localPosition.y);
                }
            }
            if (howManyBubbles == 5)
            {
                bubblesIndex = 0;
                if (bubblesArray[bubblesIndex].GetComponent<BubbleScript>().isBubbleDead == true)
                {
                    howManyBubbles -= 1;
                    Destroy(bubblesArray[bubblesIndex].gameObject);
                    bubblesArray[bubblesIndex] = null;
                }
                else
                {
                    bubblesIndex++;
                }

            }
            timeToSpawn -= Time.deltaTime;
        }
        else
        {
            if (HandleUtility.DistancePointLine(player.transform.position, river_start.transform.position, river_end.transform.position) < river_distance)
            {
                display_text.SetActive(true);
                if (Input.GetKeyDown("e"))
                {
                    Debug.Log(meow);
                    meow++;
                    fishingStarts = true;
                    display_text.SetActive(false);
                }
            }
            else
            {
                display_text.SetActive(false);
            }
        }
    }
    
}
