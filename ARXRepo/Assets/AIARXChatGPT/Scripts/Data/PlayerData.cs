using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string walletAddress;       // Player's unique wallet address

    // Resources
    public Dictionary<string, int> resources;

    // Troops
    public Dictionary<string, int> troops;

    // Commanders (Only store IDs for reference)
    public List<int> commanderIds;

    // Cards (Only store IDs for reference)
    public List<int> cardIds;

    // Buildings (Only store IDs for reference)
    public List<int> buildingIds;

    // Quests (Only store IDs for reference)
    public List<int> questIds;

    // Player State (Social and Military Rank)
    public PlayerState playerState;

    // Action Points
    public int actionPoints;

    public string occupation;  // Current occupation (e.g., Farmer, Miner, Blacksmith)


    // Time System
    public TimeData time;

    // Constructor
    public PlayerData(string walletAddress)
    {
        this.walletAddress = walletAddress;

        // Initialize Resources
        resources = new Dictionary<string, int>
        {
            { "Coins", 100 },
            { "Food", 50 },
            { "Tech", 0 },
            { "Influence", 0 }
        };

        // Initialize Troops
        troops = new Dictionary<string, int>
        {
            { "Infantry", 0 },
            { "Horsemen", 0 },
            { "Machinery", 0 }
        };

        occupation = "None";  // Default occupation

        // Initialize Lists
        commanderIds = new List<int>();
        cardIds = new List<int>();
        buildingIds = new List<int>();
        questIds = new List<int>();

        // Initialize Player State
        playerState = new PlayerState
        {
            rank = "Freeman",
            socialLevel = 1,
            militaryLevel = 1
        };

        // Initialize Action Points
        actionPoints = 3;  // Default for "Freeman"

        // Initialize Time
        time = new TimeData
        {
            day = 1,
            month = 1,
            year = 1
        };
    }
}

[Serializable]
public class PlayerState
{
    public string rank;          // Current rank (e.g., Freeman, Soldier)
    public int socialLevel;      // Social progression level
    public int militaryLevel;    // Military progression level
}

[Serializable]
public class TimeData
{
    public int day;              // Current day
    public int month;            // Current month
    public int year;             // Current year
}

