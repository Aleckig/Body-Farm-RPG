using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed; // Movement speed of the player
    [SerializeField] private Animator animator; // Animator component for controlling player animations
    [SerializeField] private SpriteRenderer playerSprite; // SpriteRenderer component for controlling player sprite
    [SerializeField] private LayerMask grassLayer; // Layer mask for detecting grass
    [SerializeField] private int stepsInGrass; // Number of steps taken by the player in grass
    [SerializeField] private int minStepsToEncounter; // Minimum steps required to trigger an encounter
    [SerializeField] private int maxStepsToEncounter; // Maximum steps required to trigger an encounter

    // PlayerControls object for handling player input
    private PlayerControls playerControls;
    private Rigidbody rb; // Rigidbody component for controlling player movement
    private Vector3 movement; // Direction of player movement
    private bool movingInGrass; // Flag indicating if the player is moving in grass
    private float stepTimer; // Timer for tracking steps taken in grass
    private int stepsToEncounter; // Number of steps required to trigger an encounter

    private const string IS_WALK_PARAM = "IsWalk"; // Animator parameter for controlling walk animation
    private const string BATTLE_SCENE = "BattleScene01"; // Name of the battle scene
    private const float Time_Per_Step = 0.5f; // Time interval between steps

    private void Awake()
    {
        // Initialize PlayerControls
        playerControls = new PlayerControls();
        // Calculate initial steps to encounter
        CalculateStepsToEncounter();    
    }

    private void OnEnable()
    {
        // Enable PlayerControls input actions
        playerControls.Enable();
    }

    private void Start()
    {
        // Get Rigidbody component
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Read player input for movement
        float moveX = playerControls.Player.Move.ReadValue<Vector2>().x;
        float moveZ = playerControls.Player.Move.ReadValue<Vector2>().y;

        // Normalize movement vector
        movement = new Vector3(moveX, 0, moveZ).normalized;

        // Set walk animation parameter
        animator.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        // Flip player sprite based on movement direction
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
        // Move the player based on movement input
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // Check if the player is moving in grass
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

        // If the player is moving in grass, track steps and check for encounters
        if(movingInGrass == true)
        {
            stepTimer += Time.deltaTime;
            if(stepTimer > Time_Per_Step)
            {
                stepTimer = 0;
                stepsInGrass++;

                // Check if the required number of steps has been taken for an encounter
                if(stepsInGrass >= stepsToEncounter)
                {
                    // Load the battle scene
                    SceneManager.LoadScene(BATTLE_SCENE);  
                }
            }
        }
        else // Reset step tracking if not in grass
        {
            stepTimer = 0;
            stepsInGrass = 0;
        }
    }

    // Calculate a random number of steps required for an encounter
    private void CalculateStepsToEncounter()
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }
}
