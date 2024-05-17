using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battle Entities")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    
    private PartyManager partyManager;
    private EnemyManager enemyManager;


    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        CreatePartEntities();
        CreateEnemyEntities();
        
    }

    private void CreatePartEntities()
    {
        //get current party members
        List<PartyMember> currentParty = new List<PartyMember>();
        currentParty = partyManager.GetCurrentParty();

        //create battle entities for party members
        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            //assign valeus to the new entity
            tempEntity.SetEntityValues(currentParty[i].MemberName, currentParty[i].Level, currentParty[i].MaxHealth, currentParty[i].CurrentHealth, currentParty[i].Strength, currentParty[i].Iniative, true);
            
            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }

        
    }

    private void CreateEnemyEntities()
    {
        //get current enemies
        List<Enemy> currentEnemies = new List<Enemy>();
        currentEnemies = enemyManager.GetCurrentEnemies();

        //create battle entities for enemies
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            //assign values to the new entity
            tempEntity.SetEntityValues(currentEnemies[i].EnemyName, currentEnemies[i].Level, currentEnemies[i].MaxHealth, currentEnemies[i].CurrentHealth, currentEnemies[i].Strength, currentEnemies[i].Iniative, false);
            
            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
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
