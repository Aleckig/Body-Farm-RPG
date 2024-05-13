using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleVisuals : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private int currentHealth;
    private int maxHealth;
    private int level;
    private Animator animator;

    private const string LEVEL_ABB = "Lvl:";
    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DEAD_PARAM = "IsDead";

    void Start()
    {
        animator = GetComponent<Animator>();
        //healthBar = GetComponent<Slider>();
        //levelText = GetComponent<TextMeshProUGUI>();
        
    }

    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
       this.currentHealth = currentHealth;
       this.maxHealth = maxHealth;
       this.level = level;

       levelText.text = LEVEL_ABB + this.level.ToString();
       UpdateHealthBar();
    }

    public void ChangeHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;

        if(currentHealth <= 0)
        {
            PlayDeathAnimation();
            Destroy(gameObject, 1.5f);
        }
        else
        {
            PlayHitAnimation();
        }
        UpdateHealthBar();
        
    }
    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(IS_ATTACK_PARAM);
    }

    public void PlayHitAnimation()
    {
        animator.SetTrigger(IS_HIT_PARAM);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger(IS_DEAD_PARAM);
    }

    
    
}
