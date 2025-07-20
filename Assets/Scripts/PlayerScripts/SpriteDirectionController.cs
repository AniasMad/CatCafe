using Unity.VisualScripting;
using UnityEngine;

public class SpriteDirectionController : MonoBehaviour
{
    [SerializeField] float backAngle = 65f;
    [SerializeField] float sideAngle = 155f;
    [SerializeField] Transform mainTransform;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    bool moving = false;

    void LateUpdate()
    {
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

        float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);

        Vector2 animationDirection = new Vector2(0f, -1f);

        float angle = Mathf.Abs(signedAngle);
        
        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (angle < backAngle)
        {
            if (moving)
            {
                animationDirection = new Vector2(-0.5f, -1f);
            }
            else
            {
                animationDirection = new Vector2(0f, -1f);
            }
        }
        else if (angle < sideAngle)
        {
            if (moving)
            {
                if (signedAngle < 0)
                {
                    animationDirection = new Vector2(-1f, -0.5f);
                }
                else
                {
                    animationDirection = new Vector2(1f, 0.5f);
                }
            }
            else
            {
                if (signedAngle < 0)
                {
                    animationDirection = new Vector2(-1f, 0);
                }
                else
                {
                    animationDirection = new Vector2(1f, 0);
                }
            }

        }
        else
        {
            if (moving)
            {
                animationDirection = new Vector2(0.5f, 1f);
            }
            else
            {
                animationDirection = new Vector2(0f, 1f);
            }
        }

        animator.SetFloat("moveX", animationDirection.x);
        animator.SetFloat("moveY", animationDirection.y);
    }
}
