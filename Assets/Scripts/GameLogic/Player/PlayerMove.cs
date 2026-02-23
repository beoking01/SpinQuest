using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3.0f;
    public float rotateSpeed = 1.0f;
    private float footstepTimer = 0f;
    public float footstepDelay = 0.25f;
    public bool isGround = true;
    [SerializeField] private LayerMask groundLayer;     
    [SerializeField] private Transform groundCheck;
    private Animator animator;
    private GameManager gameManager;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        if (gameManager.IsPlay())
        {
            MoveProcess();
        }
        UpdateAnimation();
        ProcessDirectionGravity();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGame();
        }
    }
    public void MoveProcess()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if(isGround)
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1.25f, 1.25f, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1.25f, 1.25f, 1);
        
        if (moveInput != 0)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                SoundEventManager.PLaySound(SoundType.Footstep);
                footstepTimer = footstepDelay;
            }
        }
    }
    public void UpdateAnimation()
    {
        bool isRunning = Math.Abs(rb.linearVelocity.x) > 0.1f;
        animator.SetBool("isRunning", isRunning);
    }
    public void ProcessDirectionGravity()
    {
        // Update direction of player depen on gravity
        Vector2 grav = Physics2D.gravity;
        if (grav.sqrMagnitude < 1e-6f) grav = Vector2.down * 9.81f;
        float targetAngle = Mathf.Atan2(grav.y, grav.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }
    
    public bool IsGrounded()
    {
        return isGround;
    }
}
