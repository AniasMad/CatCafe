using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;

    Rigidbody rigidBody;
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;

    private Vector3 _input;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();

        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    private void Update() {
        GatherInput();
        Look();
    }

    private void FixedUpdate() {
        Move();
        stepClimb();
    }

    private void GatherInput() {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look() {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move() {
        _rb.MovePosition(transform.position + transform.forward * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    void stepClimb()
    {
         RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

         RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f,0,1), out hitUpper45, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, 0.2f))
            {
                rigidBody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }

}

public static class Helpers 
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}

