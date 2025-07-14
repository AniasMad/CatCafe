using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventoryFish : MonoBehaviour
{
    public static int fish1 = 0, fish2 = 0, fish3 = 0;
    [SerializeField] private TMP_Text field1;
    [SerializeField] private TMP_Text field2;
    [SerializeField] private TMP_Text field3;
    // Update is called once per frame
    public void FixedUpdate()
    {
        field1.text = "x" + fish1;
        field2.text = "x" + fish2;
        field3.text = "x" + fish3;
    }
    public void Update()
    {
      if (Input.GetKeyDown("r") || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            fish1 = 0;
            fish2 = 0;
            fish3 = 0;
        }  
    }
    public static void increaseFish(int x)
    {
        switch (x)
        {
            case 1:
                fish1++;
                break;
            case 2:
                fish2++;
                break;
            case 3:
                fish3++;
                break;
            default:
                break;
        }
    }
}
