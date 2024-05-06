using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private int stepsInGrass;
    [SerializeField] private int minStepsToEncounter;
    [SerializeField] private int maxStepsToEncounter;


    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInGrass;
    private float stepTimer;
    private int stepsToEncounter;


    private const string IS_WALK_PARAM = "IsWalk";
    private const float timePerStep = 0.5f;


    private void Awake()
    {
        playerControls = new PlayerControls();
        CalculateStepsToEncounter();    
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

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

        if(movingInGrass == true)
        {
            stepTimer += Time.deltaTime;
            if(stepTimer > timePerStep)
            {
                stepTimer = 0;
                stepsInGrass++;

                //check to see if we have reached an encounter to switch to battle scene
                if(stepsInGrass >= stepsToEncounter)
                {
                    Debug.Log("Encounter");
                    
                }
            }
        }
        else
        {
            stepTimer = 0;
            stepsInGrass = 0;
        }
    }

    private void CalculateStepsToEncounter()
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }
}
