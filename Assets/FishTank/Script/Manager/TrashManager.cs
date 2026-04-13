using System.Collections.Generic;
using UnityEngine;

public class TrashManager : Singleton<TrashManager>
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Trash trashPrefab;

    [SerializeField] private List<Trash> trashList;
    [SerializeField] private List<TrashData> trashData;
    [SerializeField] private List<SpecsTrashByType> trashType;

    [Header("Meta Data")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float avoidanceForce = 5f;
    [SerializeField] private float avoidanceRadius = 1.2f;
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    void Start()
    {
        
    }

    void Update()
    {

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
        newTrash.InitMetaData(minSpeed, maxSpeed, avoidanceForce, avoidanceRadius, minBounds, maxBounds);
        return newTrash;
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
}

[System.Serializable]
public class SpecsTrashByType
{
    public string typeName;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
}