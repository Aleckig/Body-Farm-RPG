using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;


    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;


    private const string IS_WALK_PARAM = "IsWalk";


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        float moveX = playerControls.Player.Move.ReadValue<Vector2>().x;
        float moveZ = playerControls.Player.Move.ReadValue<Vector2>().y;

        movement = new Vector3(moveX, 0, moveZ).normalized;

        animator.SetBool(IS_WALK_PARAM, movement != Vector3.zero);


        // flip player sprite
        if(moveX != 0 && moveX < 0)
        {
            playerSprite.flipX = true;
        }
        
        if(moveX != 0 && moveX > 0)
        {
            playerSprite.flipX = false;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
