using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;
    [SerializeField] private float cooldownSpawner;
    private float currentCooldownSpawner;

    [SerializeField] private Vector2 minRange;
    [SerializeField] private Vector2 maxRange;

    [SerializeField] private int maxAttempts = 100;
    [SerializeField] private float spawnCheckRadius = 1.5f;

    void Awake()
    {
        LoadDataFromConfig();
    }

    private void OnEnable()
    {
        gameEvent.OnAddFish += AddDataFish;
    }

    private void OnDisable()
    {
        gameEvent.OnAddFish -= AddDataFish;
    }

    void Start()
    {
        currentCooldownSpawner = cooldownSpawner;
    }

    void Update()
    {
        if (currentCooldownSpawner <= 0)
        {
            SpawnFish();
            currentCooldownSpawner = cooldownSpawner;
        }
        else
        {
            currentCooldownSpawner -= Time.deltaTime;
        }
    }

    private void SpawnFish()
    {
        Fish newFish = FishManager.Instance.GotFish();

        if (newFish != null)
        {
            Vector3 randomPosition = GetSafeSpawnPosition();

            newFish.transform.position = randomPosition;
            newFish.gameObject.SetActive(true);
            newFish.InitData(FishManager.Instance.RandomFishData());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minRange.x + maxRange.x) / 2, (minRange.y + maxRange.y) / 2, 0);
        Vector3 size = new Vector3(maxRange.x - minRange.x, maxRange.y - minRange.y, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }

    void LoadDataFromConfig()
    {
        saveEvent.OnLoadFishSpawnerCooldown += (value) => { cooldownSpawner = value; };
        saveEvent.OnLoadFishSpawnerMinRange += (value) => { minRange = value; };
        saveEvent.OnLoadFishSpawnerMaxRange += (value) => { maxRange = value; };
    }

    void AddDataFish(FishData newFishData)
    {
        Fish newFish = FishManager.Instance.GotFish();

        if (newFish != null)
        {
            Vector3 randomPosition = GetSafeSpawnPosition();

            newFish.transform.position = randomPosition;
            newFish.gameObject.SetActive(true);
            newFish.InitData(newFishData);
        }
    }

    Vector3 GetSafeSpawnPosition()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(minRange.x, maxRange.x), Random.Range(minRange.y, maxRange.y), 0
            );

            if (Physics.OverlapSphere(randomPos, spawnCheckRadius).Length == 0)
            {
                return randomPos;
            }
        }

        return Vector3.zero; // Error code
    }
}