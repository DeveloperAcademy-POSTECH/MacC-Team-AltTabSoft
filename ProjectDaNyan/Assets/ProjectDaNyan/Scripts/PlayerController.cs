using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 50f;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ControllPlayer();
    }

    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
                rb.AddForce(0, jumpForce, 0);
                canJump = Time.time + timeBeforeNextJump;
        }
    }
}
