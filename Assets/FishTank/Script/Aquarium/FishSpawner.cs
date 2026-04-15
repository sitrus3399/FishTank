using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;
    [SerializeField] private float cooldownSpawner;
    private float currentCooldownSpawner;

    [SerializeField] private Vector2 minRange;
    [SerializeField] private Vector2 maxRange;

    void Awake()
    {
        LoadDataFromConfig();
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
            float randomX = Random.Range(minRange.x, maxRange.x);
            float randomY = Random.Range(minRange.y, maxRange.y);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);

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
        saveEvent.OnLoadFishSpawnerMinRange += (value) => { maxRange = value; };
    }
}