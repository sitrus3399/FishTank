using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    [Header("Movement Stats")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private float currentMoveSpeed;
    private Vector2 wanderDirection;

    [Header("Data")]
    private TrashData trashData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Avoidance")]
    [SerializeField] private float avoidanceForce = 5f;
    [SerializeField] private float avoidanceRadius = 1.2f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Boundary Settings")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private Vector2 cenderBounds;

    private Rigidbody2D rb;
    private bool isBouncingBack = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void OnEnable()
    {
        gameEvent.OnRemoveTrash += RemoveTrash;
    }

    private void OnDisable()
    {
        gameEvent.OnRemoveTrash -= RemoveTrash;
    }

    void Start()
    {
        currentMoveSpeed = Random.Range(minSpeed, maxSpeed);
        wanderDirection = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        if (IsAtEdge())
        {
            isBouncingBack = true;
            wanderDirection = (cenderBounds - (Vector2)transform.position).normalized;
        }
        else
        {
            if (Random.value < 0.01f)
            {
                wanderDirection = (Random.insideUnitCircle + (Vector2)transform.right).normalized;
            }
        }

        MoveTowards(wanderDirection);
        ApplyMovement();
    }

    void RemoveTrash(Trash target)
    {
        if (target == this)
        {
            gameObject.SetActive(false);
        }
    }

    bool IsAtEdge()
    {
        return transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
                   transform.position.y < minBounds.y || transform.position.y > maxBounds.y;
    }

    void MoveTowards(Vector2 targetDir, float? customSpeed = null)
    {
        float speed = customSpeed ?? currentMoveSpeed;
        Vector2 finalDirection = targetDir;

        if (!isBouncingBack)
        {
            Vector2 avoidance = CalculateAvoidance() * avoidanceForce;
            finalDirection = (targetDir + avoidance).normalized;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, finalDirection * speed, Time.deltaTime * rotationSpeed);
        }
    }

    Vector2 CalculateAvoidance() //Jika ini berat akan dijadikan collider biasa yang bertabrakan tidak tumpang tindih
    {
        Vector2 avoidanceVec = Vector2.zero;
        Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, obstacleMask);

        foreach (var col in obstacles)
        {
            if (col.gameObject == gameObject)
            {
                return avoidanceVec;
            }

            Vector2 diff = (Vector2)transform.position - (Vector2)col.transform.position;
            avoidanceVec += diff.normalized / diff.magnitude;
            //Debug.Log($"avoidanceVec {avoidanceVec} diff {diff}");
        }
        return avoidanceVec;
    }

    void ApplyMovement()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            rb.SetRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));

            float flipY = (rb.linearVelocity.x < 0) ? -1f : 1f;
            transform.localScale = new Vector3(1, flipY, 1);
        }
    }

    public void InitMetaData(float newAvoidanceForce, float newAvoidanceRadius, Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        avoidanceForce = newAvoidanceForce;
        avoidanceRadius = newAvoidanceRadius;
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }

    public void InitData(TrashData newTrashData)
    {
        trashData = newTrashData;

        SpecsTrashByType specs = TrashManager.Instance.GetSpecsByType(trashData.trashType);
        if (specs != null)
        {
            minSpeed = specs.minSpeed;
            maxSpeed = specs.maxSpeed;
        }

        currentMoveSpeed = Random.Range(minSpeed, maxSpeed);
        spriteRenderer.sprite = trashData.trashSprite;
    }
}