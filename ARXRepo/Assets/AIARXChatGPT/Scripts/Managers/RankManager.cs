using UnityEngine;

public class RankManager : MonoBehaviour
{
    public static RankManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool CanRankUp(PlayerData playerData)
    {
        return playerData.resources["Coins"] >= GetRankUpRequirement(playerData.playerState.rank) &&
               playerData.resources["Influence"] >= GetRankInfluenceRequirement(playerData.playerState.rank);
    }

    public void RankUp(PlayerData playerData)
    {
        if (!CanRankUp(playerData)) return;

        playerData.playerState.rank = playerData.playerState.rank switch
        {
            "Freeman" => "Soldier",
            "Soldier" => "Sergeant",
            "Sergeant" => "Captain",
            "Captain" => "Major",
            "Major" => "Colonel",
            "Colonel" => "General",
            "General" => "Count",
            "Count" => "Earl",
            "Earl" => "Duke",
            "Duke" => "King/Queen",
            "King/Queen" => "Emperor",
            _ => playerData.playerState.rank,
        };

        playerData.actionPoints = GetMaxActionPoints(playerData.playerState.rank);
        Debug.Log($"[RankManager] Player ranked up to {playerData.playerState.rank}.");
    }

    private int GetRankUpRequirement(string rank)
    {
        return rank switch
        {
            "Freeman" => 500,
            "Soldier" => 1500,
            "Sergeant" => 5000,
            "Captain" => 10000,
            "Major" => 20000,
            "Colonel" => 40000,
            "General" => 70000,
            "Count" => 100000,
            "Earl" => 150000,
            "Duke" => 200000,
            "King/Queen" => 300000,
            "Emperor" => 500000,
            _ => 0,
        };
    }

    private int GetRankInfluenceRequirement(string rank)
    {
        return rank switch
        {
            "Freeman" => 25,
            "Soldier" => 50,
            "Sergeant" => 100,
            "Captain" => 150,
            "Major" => 200,
            "Colonel" => 300,
            "General" => 400,
            "Count" => 500,
            "Earl" => 600,
            "Duke" => 800,
            "King/Queen" => 1000,
            "Emperor" => 1500,
            _ => 0,
        };
    }

    public int GetMaxActionPoints(string rank)
    {
        return rank switch
        {
            "Freeman" => 3,
            "Soldier" => 5,
            "Sergeant" => 7,
            "Captain" => 10,
            "Major" => 12,
            "Colonel" => 15,
            "General" => 20,
            "Count" => 25,
            "Earl" => 30,
            "Duke" => 35,
            "King/Queen" => 40,
            "Emperor" => 50,
            _ => 3,
        };
    }
}
