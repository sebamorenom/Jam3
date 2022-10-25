using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    public GameObject radialMenu;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            radialMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            radialMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
