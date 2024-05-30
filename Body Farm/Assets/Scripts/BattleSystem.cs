using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private enum BattleState { Start, Selection, Battle, Won, Lost, Run}
    
    [Header("Battle State")]
    [SerializeField] private BattleState state;

    // Serialized fields for setting spawn points in the Unity Inspector
    [Header("Spawn Points")]
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    // Lists to manage different battlers (all, enemies, and players)
    [Header("Battlers")]
    [SerializeField] private List<BattleEntities> allBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> enemyBattlers = new List<BattleEntities>();
    [SerializeField] private List<BattleEntities> playerBattlers = new List<BattleEntities>();

    // UI elements for managing battle menus and action texts
    [Header("UI")]
    [SerializeField] private GameObject[] enemySelectionButtons;
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private GameObject BattleTextPopup;
    [SerializeField] private TextMeshProUGUI battleDamageText;
    

    // Managers for party and enemies
    private PartyManager partyManager;
    private EnemyManager enemyManager;

    // Index to track the current player making a decision
    private int currentPlayer;

    // Constant string for action message
    private const string ACTION_MESSAGE = "'s Action:";
    private const string WIN_MESSAGE = "You won the battle!";
    private const int TURN_DURATION = 2;

    // Start is called before the first frame update
    void Start()
    {
        // Find and set references to party and enemy managers
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();
        enemyManager = GameObject.FindFirstObjectByType<EnemyManager>();

        // Create entities for the party and enemies
        CreatePartyEntities();
        CreateEnemyEntities();
        // Show the battle menu initially
        ShowBattleMenu();
        
    }

    private IEnumerator BattleRoutine()
    {
        //enemy selection menu disabled
        enemySelectionMenu.SetActive(false);
        //change state to battle
        state = BattleState.Battle;
        //enable popup text
        BattleTextPopup.SetActive(true);
        
        //loop through all battlers
        for (int i = 0; i < allBattlers.Count; i++)
        {
            switch (allBattlers[i].BattleAction)
            {
                case BattleEntities.Action.Attack:
                    //attack action
                    yield return StartCoroutine(AttackRoutine(i));
                    break;
                case BattleEntities.Action.Run:
                    break;
                default:
                    Debug.LogError("Invalid action selected.");
                    break;
            }
        }

        if(state == BattleState.Battle)
        {
            BattleTextPopup.SetActive(false);
            currentPlayer = 0;
            ShowBattleMenu();
            
        }
        yield return null;

    }

    // Method to create party entities and set their initial values and visuals
    private void CreatePartyEntities()
    {
        List<PartyMember> currentParty = partyManager.GetCurrentParty();

        // Ensure we have enough spawn points
        if (currentParty.Count > partySpawnPoints.Length)
        {
            Debug.LogError("Not enough party spawn points for the current party members.");
            return;
        }

        for (int i = 0; i < currentParty.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();

            tempEntity.SetEntityValues(
                currentParty[i].MemberName, 
                currentParty[i].CurrentHealth, 
                currentParty[i].MaxHealth,
                currentParty[i].Initiative, 
                currentParty[i].Strength, 
                currentParty[i].Level, 
                true
            );

            // Instantiate and set up battle visuals
            BattleVisuals tempBattleVisuals = Instantiate(
                currentParty[i].MemberBattleVisualPrefab,
                partySpawnPoints[i].position, 
                Quaternion.identity
            ).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(
                currentParty[i].MaxHealth, 
                currentParty[i].MaxHealth, 
                currentParty[i].Level
            );

            tempEntity.BattleVisuals = tempBattleVisuals;

            // Add to lists for all battlers and player battlers
            allBattlers.Add(tempEntity);
            playerBattlers.Add(tempEntity);
        }
    }

    private IEnumerator AttackRoutine(int i )
    {
        //check if the current battler is a player
        if(allBattlers[i].IsPlayer == true)
        {
            BattleEntities currentAttacker = allBattlers[i];
            BattleEntities currentTarget = allBattlers[currentAttacker.Target];
            
            AttackAction(currentAttacker, currentTarget);
            yield return new WaitForSeconds(TURN_DURATION);

            //kill the enemy
            if (currentTarget.CurrentHealth <= 0)
            {
                battleDamageText.text = string.Format("{0} defeated {1}", currentAttacker, currentTarget.Name);
                yield return new WaitForSeconds(TURN_DURATION);
                enemyBattlers.Remove(currentTarget);
                allBattlers.Remove(currentTarget);
                //Destroy(currentTarget.BattleVisuals.gameObject);
                if(enemyBattlers.Count <= 0)
                {
                    state = BattleState.Won;
                    //Debug.Log("You won the battle!");
                    battleDamageText.text = WIN_MESSAGE;
                    yield return new WaitForSeconds(TURN_DURATION);
                    //end the battle and switch scene


                }
            }


        }
        
    }


    // Method to create enemy entities and set their initial values and visuals
    private void CreateEnemyEntities()
    {
        List<Enemy> currentEnemies = enemyManager.GetCurrentEnemies();
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            BattleEntities tempEntity = new BattleEntities();
            tempEntity.SetEntityValues(
                currentEnemies[i].EnemyName,
                currentEnemies[i].CurrentHealth,
                currentEnemies[i].MaxHealth,
                currentEnemies[i].Initiative,
                currentEnemies[i].Strength,
                currentEnemies[i].Level,
                false
            );

            BattleVisuals tempBattleVisuals = Instantiate(
                currentEnemies[i].EnemyVisualPrefab,
                enemySpawnPoints[i].position,
                Quaternion.identity
            ).GetComponent<BattleVisuals>();

            tempBattleVisuals.SetStartingValues(
                currentEnemies[i].MaxHealth,
                currentEnemies[i].MaxHealth,
                currentEnemies[i].Level
            );

            tempEntity.BattleVisuals = tempBattleVisuals;

            // Add to lists for all battlers and enemy battlers
            allBattlers.Add(tempEntity);
            enemyBattlers.Add(tempEntity);
        }
    }

    // Method to show the battle menu for the current player
    public void ShowBattleMenu()
    {
        actionText.text = playerBattlers[currentPlayer].Name + ACTION_MESSAGE;
        battleMenu.SetActive(true);
    }

    // Method to show the enemy selection menu
    public void ShowEnemySelectionMenu()
    {
        battleMenu.SetActive(false);
        SetEnemySelectionButtons();
        enemySelectionMenu.SetActive(true);
    }

    // Method to set up enemy selection buttons based on available enemies
    private void SetEnemySelectionButtons()
    {
        // Disable all enemy selection buttons initially
        for (int i = 0; i < enemySelectionButtons.Length; i++)
        {
            enemySelectionButtons[i].SetActive(false);
        }

        // Enable buttons and set their text for each enemy
        for (int j = 0; j < enemyBattlers.Count; j++)
        {
            enemySelectionButtons[j].SetActive(true);
            enemySelectionButtons[j].GetComponentInChildren<TextMeshProUGUI>().text = enemyBattlers[j].Name;
        }
    }

    // Method to handle enemy selection by the current player
    public void SelectEnemy(int currentEnemy)
    {
        BattleEntities currentPlayerEntity = playerBattlers[currentPlayer];
        currentPlayerEntity.SetTarget(allBattlers.IndexOf(enemyBattlers[currentEnemy]));
        currentPlayerEntity.BattleAction = BattleEntities.Action.Attack;

        currentPlayer++;

        if (currentPlayer >= playerBattlers.Count)
        {
            // Start the battle if all players have selected an action
            StartCoroutine(BattleRoutine());
            Debug.Log("We are attacking: " + allBattlers[currentPlayerEntity.Target].Name);
        }
        else
        {
            // Show the battle menu for the next player
            enemySelectionMenu.SetActive(false);
            ShowBattleMenu();
        }
    }

    private void AttackAction(BattleEntities currentAttacker, BattleEntities currentTarget)
    {
        // Calculate damage based on attacker's strength
        int damage = currentAttacker.Strength;

        //play attack animation
        currentAttacker.BattleVisuals.PlayAttackAnimation();

        // Reduce target's health by the damage amount
        currentTarget.CurrentHealth -= damage;

        //target play Hit animation
        currentTarget.BattleVisuals.PlayHitAnimation();

        // Update the target's health bar
        currentTarget.UpdateUI();
        battleDamageText.text = string.Format("{0} dealt {1} damage to {2}!", currentAttacker.Name, damage, currentTarget.Name);
    }

    
}

// Class representing a battle entity (player or enemy)
[System.Serializable]
public class BattleEntities
{
    public enum Action { Attack, Run }
    public Action BattleAction;
    public string Name;
    public int CurrentHealth;
    public int MaxHealth;
    public int Initiative;
    public int Strength;
    public int Level;
    public bool IsPlayer;
    public BattleVisuals BattleVisuals;
    public int Target;

    // Method to set initial values for the entity
    public void SetEntityValues(string name, int currHealth, int maxHealth, int initiative, int strength, int level, bool isPlayer)
    {
        Name = name;
        CurrentHealth = currHealth;
        MaxHealth = maxHealth;
        Initiative = initiative;
        Strength = strength;
        Level = level;
        IsPlayer = isPlayer;
    }

    // Method to set the target for an action
    public void SetTarget(int target)
    {
        Target = target;
    }

    public void UpdateUI()
    {
        BattleVisuals.ChangeHealth(CurrentHealth);
    }
}
