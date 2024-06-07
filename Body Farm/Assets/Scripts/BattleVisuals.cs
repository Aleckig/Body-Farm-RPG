using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar; // UI Slider component for displaying health
    [SerializeField] private TextMeshProUGUI levelText; // UI TextMeshPro component for displaying level

    private int currentHealth; // Current health of the character
    private int maxHealth; // Maximum health of the character
    private int level; // Level of the character
    private Animator animator; // Animator component for controlling animations

    // Constants for level text abbreviation and animation parameters
    private const string LEVEL_ABB = "Lvl:";
    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DEAD_PARAM = "IsDead";

    void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = gameObject.GetComponent<Animator>();   
    }

    // Method to set the initial values for health and level
    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
       this.currentHealth = currentHealth;
       this.maxHealth = maxHealth;
       this.level = level;

       // Update the level text to display the character's level
       levelText.text = LEVEL_ABB + this.level.ToString();

       // Update the health bar to reflect the current health
       UpdateHealthBar();
    }

    // Method to change the health value
    public void ChangeHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;

        // If health is 0 or less, play death animation and destroy the GameObject after 1.5 seconds
        if(currentHealth <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1.5f);
        }
        else
        {
            // If still alive, play hit animation
            PlayHitAnimation();
        }

        // Update the health bar to reflect the new health value
        UpdateHealthBar();
    }

    // Method to update the health bar
    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Method to play attack animation
    public void PlayAttackAnimation()
    {
        animator.SetTrigger(IS_ATTACK_PARAM);
    }

    // Method to play hit animation
    public void PlayHitAnimation()
    {
        animator.SetTrigger(IS_HIT_PARAM);
    }

    // Method to play death animation
    public void PlayDeathAnimation()
    {
        animator.SetTrigger(IS_DEAD_PARAM);
    }

    
    
}
