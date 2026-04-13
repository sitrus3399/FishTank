using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private float cooldownSpawner;
    private float currentCooldownSpawner;

    [SerializeField] private Vector2 minRange;
    [SerializeField] private Vector2 maxRange;

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
            float randomX = Random.Range(minRange.x, maxRange.x);
            float randomY = Random.Range(minRange.y, maxRange.y);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);

            newTrash.transform.position = randomPosition;
            newTrash.gameObject.SetActive(true);
            newTrash.InitData(TrashManager.Instance.RandomTrashData());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minRange.x + maxRange.x) / 2, (minRange.y + maxRange.y) / 2, 0);
        Vector3 size = new Vector3(maxRange.x - minRange.x, maxRange.y - minRange.y, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}