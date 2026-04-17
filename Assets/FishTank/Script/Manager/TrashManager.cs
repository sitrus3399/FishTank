using System.Collections.Generic;
using UnityEngine;

public class TrashManager : Singleton<TrashManager>
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private SaveEvent saveEvent;
    [SerializeField] private Trash trashPrefab;

    [SerializeField] private List<Trash> trashList;
    [SerializeField] private List<TrashData> trashData;
    [SerializeField] private List<SpecsTrashByType> trashType;

    [Header("Meta Data")]
    [SerializeField] private float avoidanceForce = 5f;
    [SerializeField] private float avoidanceRadius = 1.2f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    protected override void Awake()
    {
        base.Awake();
        LoadDataFromConfig();
    }

    private void OnEnable()
    {
        gameEvent.OnAddTrash += AddDataTrash;
        gameEvent.OnRemoveTrashData += RemoveDataTrash;
    }

    private void OnDisable()
    {
        gameEvent.OnAddTrash -= AddDataTrash;
        gameEvent.OnRemoveTrashData -= RemoveDataTrash;
    }

    public Trash GotTrash()
    {
        foreach (var trash in trashList)
        {
            if (!trash.gameObject.activeInHierarchy)
            {
                return trash;
            }
        }

        return CreateTrash();
    }

    public Trash CreateTrash()
    {
        Trash newTrash = Instantiate(trashPrefab);
        newTrash.gameObject.SetActive(false);
        trashList.Add(newTrash);
        newTrash.InitMetaData(avoidanceForce, avoidanceRadius, minBounds, maxBounds);
        return newTrash;
    }

    void RemoveDataTrash(TrashData newTrashData)
    {
        for (int i = trashList.Count - 1; i >= 0; i--)
        {
            if (trashList[i].TrashData == newTrashData)
            {
                GameObject objToDestroy = trashList[i].gameObject;
                trashList.RemoveAt(i);
                Destroy(objToDestroy);
            }
        }

        for (int i = trashData.Count - 1; i >= 0; i--)
        {
            if (trashData[i] == newTrashData)
            {
                trashData.RemoveAt(i);
            }
        }
    }

    public TrashData RandomTrashData()
    {
        int index = Random.Range(0, trashData.Count);
        return trashData[index];
    }

    public SpecsTrashByType GetSpecsByType(string newTypeName)
    {
        return trashType.Find(x => x.typeName == newTypeName);
    }

    void LoadDataFromConfig()
    {
        saveEvent.OnLoadSpecsTrashByType += (value) => { trashType = value; };
        saveEvent.OnLoadTrashAvoidanceForce += (value) => { avoidanceForce = value; };
        saveEvent.OnLoadTrashAvoidanceRadius += (value) => { avoidanceRadius = value; };
        saveEvent.OnLoadTrashMinBounds += (value) => { minBounds = value; };
        saveEvent.OnLoadTrashMaxBounds += (value) => { maxBounds = value; };
    }

    void AddDataTrash(TrashData newTrash)
    {
        trashData.Add(newTrash);
    }
}

[System.Serializable]
public class SpecsTrashByType
{
    public string typeName;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
}