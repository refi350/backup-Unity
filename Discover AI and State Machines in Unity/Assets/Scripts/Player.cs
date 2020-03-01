using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private Vector3 movementVector;
    private Animator animator;
    private Rigidbody body;
    private Health health;
    public float speed = 1.5f;
    public float rotateSpeed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        body = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if(movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementVector), rotateSpeed);
        }
        animator.SetBool("Walking", movementVector != Vector3.zero);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hurt(2,2);
        }
    }

    void CalculateMovement()
    {
        movementVector = new Vector3(
            Input.GetAxis("Horizontal"),
            body.velocity.y,
            Input.GetAxis("Vertical"));
        body.velocity = new Vector3(
            movementVector.x * speed,
            movementVector.y,
            movementVector.z * speed);
    }

    public void Hurt(int amount, int delay = 0)
    {
        StartCoroutine(health.TakeDamageDelayed(amount, delay));
    }
}
