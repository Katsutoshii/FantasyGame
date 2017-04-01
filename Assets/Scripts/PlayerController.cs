using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Animator animator;

    public Rigidbody rb;
    public float maxMoveSpeed;

    public enum State
    {
        idle, accelerating, deccelerating, running, turning, attacking, dashAttacking
    }

    public State state;
    public Vector3 targetDirection;
    private float moveSpeed;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = State.idle;
        targetDirection = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        switch (state)
        {
            case State.idle: //if standing still
                print("Idle!");
                animator.Play("Idle");
                HandleKey();
                break;

            case State.turning:
                print("Turning!");
                animator.Play("Run");
                Turn();
                break;

            case State.accelerating:
                print("Accelerating!");
                animator.Play("Run");
                Accelerate();
                break;

            case State.deccelerating:
                print("Deccelerating!");
                animator.Play("Walk");
                Deccelerate();
                break;

            case State.running:
                print("Running!");
                animator.Play("Run");
                HandleKey();
                if (!Input.anyKey) state = State.deccelerating;
                break;
        }
	}
    
    void HandleKey()
    {
        if (Input.GetKey("w"))
        {
            targetDirection.y = 0; //set direction to forwards
            HandleDirectionChange();
        }
        else if (Input.GetKey("d"))
        {
            targetDirection.y = 90; //set direction to right
            HandleDirectionChange();
        }
        else if (Input.GetKey("a"))
        {
            targetDirection.y = 270; //set direction to left
            HandleDirectionChange();
        }
        else if (Input.GetKey("s"))
        {
            targetDirection.y = 180; //set direction to back
            HandleDirectionChange();
        }

        if(Input.GetMouseButton(1))
        {
            if (state == State.running) state = State.dashAttacking;
            else if (state == State.idle) state = State.attacking;
        }
    }

    void HandleDirectionChange()
    {

        if (!transform.eulerAngles.Equals(targetDirection)) //if not facing the right way
        {
            if (state == State.running) state = State.deccelerating;    //stop moving if running
            else state = State.turning;                                 //turn to the right direction
        }
        else if (state == State.idle) state = State.accelerating;
    }

    void Accelerate()
    {
        moveSpeed += 1;
        rb.velocity = transform.forward * moveSpeed;
        if (moveSpeed >= maxMoveSpeed) state = State.running;
    }

    void Deccelerate()
    {
        moveSpeed -= 1;
        rb.velocity = transform.forward * moveSpeed;
        if (moveSpeed == 0) state = State.idle;
    }

    void Turn()
    {
        float dY = targetDirection.y - transform.eulerAngles.y;
        if (dY > -0.1 && dY < 0.1)
        {
            transform.eulerAngles = targetDirection;
            state = State.idle;
        }
        else if (dY < 0.0)
        {
            if (dY > -180) transform.Rotate(Vector3.up, -10);
            else transform.Rotate(Vector3.up, 10);
        }
        else if (dY > 0.0)
        {
            if (dY < 180) transform.Rotate(Vector3.up, 10);
            else transform.Rotate(Vector3.up, -10);
        }
    }
}
