using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] playerSpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("Battle Entities")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;




    private PartyManager partyManager;
    private EnemyManager enemyManager;
    private int currentPlayer;

    private const string ACTION_MESSAGE = "'s Action:";


    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        CreatePartEntities();
        CreateEnemyEntities();
        ShowBattleMenu();
        
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

            //spawn the visuals for the party members
            BattleVisuals tempBattleVisuals = Instantiate(currentParty[i].MemberBattleVisualPrefab, playerSpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            //set UI visuals starting position
            tempBattleVisuals.SetStartingValues(currentParty[i].MaxHealth,currentParty[i].MaxHealth, currentParty[i].Level);

            //assign it to the battle enteties
            tempEntity.BattleVisuals = tempBattleVisuals;

            
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

            //spawn the visuals for the enemies
            BattleVisuals tempBattleVisuals = Instantiate(currentEnemies[i].EnemyVisualPrefab, enemySpawnPoints[i].position, Quaternion.identity).GetComponent<BattleVisuals>();

            //set UI visuals starting position
            tempBattleVisuals.SetStartingValues(currentEnemies[i].MaxHealth, currentEnemies[i].MaxHealth, currentEnemies[i].Level);

            //assign it to the battle entities
            tempEntity.BattleVisuals = tempBattleVisuals;
            
            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    public void ShowBattleMenu()
    {
        //who is the current entity
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;
        battleMenu.SetActive(true);
        enemySelectionMenu.SetActive(false);
    }

    public void ShowEnemySelectionMenu()
    {
        //Disable the battle menu and enable the enemy selection menu
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    private void SetEnemySelectionButtons()
    {
        for (int i = 0; i < enemySelectionButtons.Length; i++)
        {
                enemySelectionButtons[i].SetActive(false);        
        }
        for (int j = 0; j < enemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;
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
    public BattleVisuals BattleVisuals;

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
