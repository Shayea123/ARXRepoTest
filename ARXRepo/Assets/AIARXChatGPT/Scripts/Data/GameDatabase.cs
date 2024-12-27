using UnityEngine;

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase Instance { get; private set; }

    public CardDatabase CardDatabase { get; private set; }
    public CommanderDatabase CommanderDatabase { get; private set; }
    public BuildingDatabase BuildingDatabase { get; private set; }
    public QuestDatabase QuestDatabase { get; private set; }
    public OccupationDatabase OccupationDatabase { get; private set; }
    public TroopDatabase TroopDatabase { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize individual databases
        CardDatabase = GetComponent<CardDatabase>();
        CommanderDatabase = GetComponent<CommanderDatabase>();
        BuildingDatabase = GetComponent<BuildingDatabase>();
        QuestDatabase = GetComponent<QuestDatabase>();
        OccupationDatabase = GetComponent<OccupationDatabase>();
        TroopDatabase = GetComponent<TroopDatabase>();
    }

    public T GetEntityById<T>(int id) where T : ScriptableObject
    {
        if (typeof(T) == typeof(CardData))
            return CardDatabase.GetCardById(id) as T;
        if (typeof(T) == typeof(CommanderData))
            return CommanderDatabase.GetCommanderById(id) as T;
        if (typeof(T) == typeof(BuildingData))
            return BuildingDatabase.GetBuildingById(id) as T;
        if (typeof(T) == typeof(QuestData))
            return QuestDatabase.GetQuestById(id) as T;
        if (typeof(T) == typeof(OccupationData))
            return OccupationDatabase.GetOccupationByName(id.ToString()) as T;
        if (typeof(T) == typeof(TroopData))
            return TroopDatabase.GetTroopById(id) as T;

        return null;
    }
}
