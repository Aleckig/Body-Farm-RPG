using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;


    private const float Level_Modifier = 0.5f;


    private void Awake()
    {
        GenerateEnemyByName("Slime", 1);
    }   

    private void GenerateEnemyByName(string EnemyName, int level)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (EnemyName == allEnemies[i].EnemyName )
            {
                Enemy newEnemy = new Enemy();
                newEnemy.EnemyName = allEnemies[i].EnemyName;
                newEnemy.Level = level;
                float levelModifier = (Level_Modifier * newEnemy.Level);
                
                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth +(allEnemies[i].BaseHealth * levelModifier));
                newEnemy.CurrentHealth = newEnemy.MaxHealth;
                newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStr +(allEnemies[i].BaseStr * levelModifier));
                newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative +(allEnemies[i].BaseInitiative * levelModifier));
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;
                
                currentEnemies.Add(newEnemy);
                
            }
        }
    }

    public List<Enemy> GetCurrentEnemies()
    {
        return currentEnemies;
    }
    
  
}

[System.Serializable]

public class Enemy
{
    public string EnemyName;
    public int Level;
    public int CurrentHealth;
    public int MaxHealth;
    public int Strength;
    public int Initiative;
    public GameObject EnemyVisualPrefab;

}
