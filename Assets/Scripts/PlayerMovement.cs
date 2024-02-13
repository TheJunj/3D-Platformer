using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    Vector3 InputVector;
    Vector3 VerticalVelocity;

    [SerializeField] private LayerMask jumpableGround;

    bool Grounded;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpSpeed;

    [SerializeField] private float JumpAnimationLimit;

    private enum MovementState {  idle, running, jumping, falling }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        Debug.Log(anim);
    }

    // Update is called once per frame
    void Update()
    {
        HandleJumping();
        MoveAndRotate();
        UpdateAnimation();
    }

    void HandleJumping()
    {
        Grounded = IsGrounded();
        VerticalVelocity = rb.velocity.y * Vector3.up;
        if (Input.GetButtonDown("Jump") && Grounded)
        {
            VerticalVelocity.y = JumpSpeed;
        }
    }

    void MoveAndRotate()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 CamForward = Camera.main.transform.forward;
        Vector3 CamRight = Camera.main.transform.right;

        CamForward.y = 0;
        CamRight.y = 0;

        CamForward.Normalize();
        CamRight.Normalize();

        InputVector = (zInput * CamForward) + (xInput * CamRight);

        if (InputVector.sqrMagnitude > 1)
        {
            InputVector.Normalize();
        }

        Vector3 MoveVector = (InputVector * MoveSpeed) + VerticalVelocity;

        rb.velocity = MoveVector;

        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }

    void UpdateAnimation()
    {
        MovementState state = MovementState.idle;
        if (InputVector.sqrMagnitude > 0.2f)
        {
            state = MovementState.running;
        }

        if (!Grounded)
        {
            if (rb.velocity.y > JumpAnimationLimit)
            {
                state = MovementState.jumping;
            }
            else
            {
                state = MovementState.falling;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, .5f, Vector3.down, out _, .6f, jumpableGround);
    }
}
