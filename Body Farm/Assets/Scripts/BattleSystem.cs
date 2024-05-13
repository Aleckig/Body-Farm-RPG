using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private PartyManager partyManager;
    private EnemyManager enemyManager;

    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();


    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();
        
    }

   
}

[System.Serializable]
public class BattleEntities
{
    public string Name;
    public int Level;
    public int MaxHealth;
    public int CurrentHealth;
    public int Strenght;
    public int Iniative;
    public bool IsPlayer;

    public void SetEntityValues(string name, int level, int maxHealth, int currentHealth, int strenght, int iniative, bool isPlayer)
    {
        Name = name;
        Level = level;
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        Strenght = strenght;
        Iniative = iniative;
        IsPlayer = isPlayer;
    }
    

    

}
