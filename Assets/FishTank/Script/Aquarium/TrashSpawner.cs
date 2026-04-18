using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;
    [SerializeField] private float cooldownSpawner;
    private float currentCooldownSpawner;

    [SerializeField] private Vector2 minRange;
    [SerializeField] private Vector2 maxRange;

    [SerializeField] private int maxAttempts = 100;
    [SerializeField] private float spawnCheckRadius = 1.5f;

    [SerializeField] private ParticleSystem bubbleBlast;
    [SerializeField] private List<ParticleSystem> bubbleBlastList;

    void Awake()
    {
        LoadDataFromConfig();
    }

    private void OnEnable()
    {
        gameEvent.OnAddTrash += AddDataTrash;
    }

    private void OnDisable()
    {
        gameEvent.OnAddTrash -= AddDataTrash;
    }

    void Start()
    {
        currentCooldownSpawner = cooldownSpawner;
    }

    void Update()
    {
        if (currentCooldownSpawner <= 0)
        {
            SpawnTrash();
            currentCooldownSpawner = cooldownSpawner;
        }
        else
        {
            currentCooldownSpawner -= Time.deltaTime;
        }
    }

    private void SpawnTrash()
    {
        Trash newTrash = TrashManager.Instance.GotTrash();

        if (newTrash != null)
        {
            Vector3 randomPosition = GetSafeSpawnPosition();

            newTrash.transform.position = randomPosition;
            newTrash.gameObject.SetActive(true);
            newTrash.InitData(TrashManager.Instance.RandomTrashData());

            SpawnParticle(randomPosition);
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
        saveEvent.OnLoadTrashSpawnerCooldown += (value) => { cooldownSpawner = value; };
        saveEvent.OnLoadTrashSpawnerMinRange += (value) => { minRange = value; };
        saveEvent.OnLoadTrashSpawnerMaxRange += (value) => { maxRange = value; };
    }

    void AddDataTrash(TrashData newTrashData)
    {
        Trash newTrash = TrashManager.Instance.GotTrash();

        if (newTrash != null)
        {
            Vector3 randomPosition = GetSafeSpawnPosition();

            newTrash.transform.position = randomPosition;
            newTrash.gameObject.SetActive(true);
            newTrash.InitData(newTrashData);

            SpawnParticle(randomPosition);
        }
    }

    public void SpawnParticle(Vector3 loc)
    {
        AudioManager.Instance.PlaySFX("Bubble");

        ParticleSystem bubble = GotBubbleBlast();
        bubble.gameObject.SetActive(true);
        bubble.gameObject.transform.position = loc;
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

    private ParticleSystem GotBubbleBlast()
    {
        foreach (ParticleSystem particle in bubbleBlastList)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                return particle;
            }
        }

        return CreateBubbleBlast();
    }

    public ParticleSystem CreateBubbleBlast()
    {
        ParticleSystem newParticle = Instantiate(bubbleBlast);
        newParticle.gameObject.SetActive(false);
        bubbleBlastList.Add(newParticle);
        return newParticle;
    }
}