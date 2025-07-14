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
    [SerializeField] private GameObject display_text;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject catPaw;
    private bool fishingStarts = false; // Start variable to run only first set of instructions
    private bool fishingInProgress = false; // Variable to continue fishing
    // Fishing Game Variables
    [Header("Fishing Game stuff~")]
    [SerializeField] private float bubbleSpeed = 100.0f; // defines bubble speed
    [SerializeField] private Transform spawn1; // first spawn point of bubbles
    [SerializeField] private Transform spawn2; // second spawn point of bubbles
    [SerializeField] private GameObject catchZone; // Zone where bubble can be caught
    [SerializeField] private float spawnTime = 3.0f; // How long the new bubble waits before spawning
    [SerializeField] private GameObject bubble; // Prefab for bubble Object
    private float timeToSpawn = 0f;
    private int whichSpawnPoint = 0;
    private GameObject[] bubblesArray; // array of bubble objects
    private int bubblesIndex = 0; // which bubble is created on the screen
    int bubbleWithFish = -1; // Determines which bubble index has a fish
    private string caughtFish;
    private bool isInRange = false;

    void Start()
    {

        bubblesArray = new GameObject[6];
        catchZone.SetActive(false);
        catPaw.SetActive(false);
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
                catchZone.SetActive(true);
                catPaw.SetActive(true);
            }
            if (bubbleWithFish >= 0)
            {
                bool caughtFishWithE = false;
                if (bubblesArray[bubbleWithFish] != null)
                {
                    if (bubblesArray[bubbleWithFish].gameObject.GetComponent<BubbleScript>().isInCatchZone())
                    {
                        if (Input.GetKeyDown("e") || Input.GetKeyDown(KeyCode.JoystickButton2))
                        {
                            finishFishing(true);
                            caughtFishWithE = true;
                        }
                    }
                    if (!caughtFishWithE)
                    {
                        if (bubblesArray[bubbleWithFish].gameObject.GetComponent<BubbleScript>().isOutOfCatchZone())
                        {
                            finishFishing(false);
                        }
                    }
                }
            }
        }
        else
        {
            if (isInRange)
            {
                display_text.SetActive(true);
                if (Input.GetKeyDown("e") || Input.GetKeyDown(KeyCode.JoystickButton2))
                {
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

    public void OnTriggerEnter(Collider other)
    {
        isInRange = true;
    }
    public void OnTriggerExit(Collider other)
    {
        isInRange = false;
    }

    public void FixedUpdate()
    {
        if (fishingInProgress)
        {
            if (timeToSpawn <= 0 && bubbleWithFish == -1 && bubblesArray[bubblesIndex] == null)
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
                timeToSpawn += spawnTime;
                if (bubblesArray[bubblesIndex].GetComponent<BubbleScript>().GetFishState()) bubbleWithFish = bubblesIndex;
                Debug.Log(bubblesArray[bubblesIndex].gameObject.GetComponent<BubbleScript>().GetFishState() + " is Bubble with fish");

                if (bubblesIndex == 5)
                {
                    bubblesIndex = 0;
                }
                else
                {
                    bubblesIndex++;
                }
            }
            for (int i = 0; i < bubblesArray.Length; i++)
            {
                if (bubblesArray[i] != null)
                {
                    bubblesArray[i].transform.localPosition = new Vector2(
                    bubblesArray[i].GetComponent<RectTransform>().localPosition.x + (bubbleSpeed * Time.deltaTime),
                    bubblesArray[i].GetComponent<RectTransform>().localPosition.y);
                }
            }
            timeToSpawn -= Time.deltaTime;

        }
    }
    private void finishFishing(bool caught)
    {
        // set variables to default
        caughtFish = bubblesArray[bubbleWithFish].GetComponent<BubbleScript>().NameGetter();
        Destroy(bubblesArray[bubbleWithFish]);
        fishingInProgress = false;
        bubblesArray = new GameObject[6];
        bubblesIndex = 0;
        timeToSpawn = 0f;
        whichSpawnPoint = 0;
        bubbleWithFish = -1;
        catPaw.GetComponent<Animator>().SetBool("pawDisappears", true);
        //StartCoroutine(WaitForPaw(10.0f));

        catchZone.SetActive(false);
        if (caught)
        {
            switch (caughtFish)
            {
                case "Small Yellow Eye":
                    InventoryFish.increaseFish(1);
                    break;
                case "Wide Mouth Pink":
                    InventoryFish.increaseFish(2);
                    break;
                case "Slimy Blub":
                    InventoryFish.increaseFish(3);
                    break;
                default:
                    break;
            }
        }
    }
}
