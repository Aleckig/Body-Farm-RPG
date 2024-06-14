using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private PartyMemberInfo[] allMembers; // Array to store information about all party members
    [SerializeField] private List<PartyMember> currentParty; // List to store information about current party members
    [SerializeField] private PartyMemberInfo defaultPartyMember; // Default party member to add to the party

    private Vector3 playerPosition; // Position of the player in the overworld
    private static GameObject instance; // Singleton instance of the PartyManager

    private void Awake()
    {
        //AddMemberToPartyByName(defaultPartyMember.MemberName);
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this.gameObject;
            AddMemberToPartyByName(defaultPartyMember.MemberName);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Method to add a member to the party by name
    public void AddMemberToPartyByName(string memberName)
    {
        // Iterate through all party members
        for (int i = 0; i < allMembers.Length; i++)
        {
            // Check if the current party member matches the provided name
            if (allMembers[i].MemberName == memberName)
            {
                // Create a new PartyMember object and populate its fields with data from the PartyMemberInfo object
                PartyMember newPartyMember = new PartyMember();
                newPartyMember.MemberName = allMembers[i].MemberName;
                newPartyMember.Level = allMembers[i].StartingLevel;
                newPartyMember.CurrentHealth = allMembers[i].BaseHealth;
                newPartyMember.MaxHealth = newPartyMember.CurrentHealth;
                newPartyMember.Strength = allMembers[i].BaseStr;
                newPartyMember.Initiative = allMembers[i].BaseInitiative;
                newPartyMember.MemberBattleVisualPrefab = allMembers[i].MemberBattleVisualPrefab;
                newPartyMember.MemberOverworldVisualPrefab = allMembers[i].MemberOverworldVisualPrefab;

                // Add the newly created PartyMember object to the current party list
                currentParty.Add(newPartyMember);
            }
        }
    }

    public List<PartyMember> GetCurrentParty()
    {
        return currentParty;
    }
    public void SaveHealth(int partyMember, int health)
    {
        currentParty[partyMember].CurrentHealth = health;
    }

    public void SetPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public Vector3 GetPosition()
    {
        return playerPosition;
    }
    
}
// Class to represent a party member
[System.Serializable]
public class PartyMember
{
    public string MemberName; // Name of the party member
    public int Level; // Level of the party member
    public int CurrentHealth; // Current health of the party member
    public int MaxHealth; // Maximum health of the party member
    public int Strength; // Strength of the party member
    public int Initiative; // Initiative of the party member
    public int CurrentEXP; // Current experience points of the party member 
    public int MaxEXP; // Maximum experience points of the party member 
    public GameObject MemberBattleVisualPrefab; // Prefab for the battle visual representation of the party member
    public GameObject MemberOverworldVisualPrefab; // Prefab for the overworld visual representation of the party member
}