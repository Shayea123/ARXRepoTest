using System.Collections.Generic;
using System;

[Serializable]
public class PlayerData
{
    public string walletAddress;

    public int coins;
    public int food;
    public int technology;        // Technology points
    public int infantry;
    public int knights;
    public int ram;               // Siege weapon

    public string rank;
    public int actionPoints;      // Remaining Action Points
    public int day;               // Current day
    public int month;             // Current month
    public int year;              // Current year
    public int influence;

    public bool isEnslaved;       // Whether the player is enslaved

    public string occupation;     // Current occupation (Farmer, Blacksmith, etc.)
    public List<string> specialCards;  // List of owned special cards
    public List<Commander> commanders; // List of assigned commanders

    // Constructor for initializing new data
    public PlayerData(string walletAddress)
    {
        this.walletAddress = walletAddress;
        coins = 100;
        food = 50;
        technology = 0;

        infantry = 0;
        knights = 0;
        ram = 0;

        rank = "Freeman";
        actionPoints = 3;      // Default AP for Freeman
        day = 1;
        month = 1;
        year = 1;

        influence = 0;
        isEnslaved = false;

        occupation = "None";  // No occupation initially
        specialCards = new List<string>();
        commanders = new List<Commander>();
    }
}

// Commander Class
[Serializable]
public class Commander
{
    public int id;                 // Unique ID for each commander
    public string name;            // Commander name
    public string specialty;       // Specialty (e.g., Viking, Archer, etc.)
    public bool assigned;          // Whether the commander is assigned to a task
}
