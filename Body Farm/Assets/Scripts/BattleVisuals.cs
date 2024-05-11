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

    private const string LEVEL_ABB = "Lvl:";

    void Start()
    {
        healthBar = GetComponent<Slider>();
        levelText = GetComponent<TextMeshProUGUI>();
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
        
    }
    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    
    
}
