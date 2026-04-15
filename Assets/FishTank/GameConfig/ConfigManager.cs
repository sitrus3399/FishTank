using UnityEngine;
using System.IO;

public class ConfigManager : MonoBehaviour
{
    public GameConfig config;
    public GameConfig defaultConfig;
    private string configFileName = "config.json";
    [SerializeField] private SaveEvent saveEvent;

    void Start()
    {
        LoadConfiguration();
    }

    [ContextMenu("Reload Config")]
    public void LoadConfiguration()
    {
        string directoryPath = Application.streamingAssetsPath;
        string filePath = Path.Combine(Application.streamingAssetsPath, configFileName);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            config = JsonUtility.FromJson<GameConfig>(jsonContent);
            Debug.Log("Konfigurasi Berhasil Dimuat: " + config.gameTitle);
        }
        else
        {
            config = defaultConfig;

            string jsonToSave = JsonUtility.ToJson(config, true);
            File.WriteAllText(filePath, jsonToSave);

            Debug.Log("Default config created with descriptions.");
        }

        saveEvent.LoadFishDataManager(config.specsFishByType, config.fishDetectionRadius, config.fishHungerMeterMax, config.fishHungerCooldown, config.fishAvoidanceForce, config.fishAvoidanceRadius, config.fishMinBounds, config.fishMaxBounds);
        saveEvent.LoadFishDataSpawner(config.fishSpawnerCooldown, config.fishSpawnerMinRange, config.fishSpawnerMaxRange);

        saveEvent.LoadTrashDataManager(config.specsTrashByType, config.trashAvoidanceForce, config.trashAvoidanceRadius, config.trashMinBounds, config.trashMaxBounds);
        saveEvent.LoadTrashDataSpawner(config.trashSpawnerCooldown, config.trashSpawnerMinRange, config.trashSpawnerMaxRange);
    }
}