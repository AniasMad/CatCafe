using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public bool isBubbleColliding = false;
    public bool isBubbleDead = false;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision ayyyy");
        isBubbleColliding = true;
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            Debug.Log("Die rn lmao");
            isBubbleDead = true;
        }
        isBubbleColliding = false;
    }
}
