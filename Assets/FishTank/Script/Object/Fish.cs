using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Fish : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    [Header("Movement Stats")]
    [SerializeField] private FishState fishState;
    [SerializeField] private float minSpeed = 2f; //For default 
    [SerializeField] private float maxSpeed = 5f; //For default
    private float currentMoveSpeed;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private Vector2 wanderDirection;
    [SerializeField] private float minAccelerateTime;
    [SerializeField] private float maxAccelerateTime;
    private float currentAccelerateTime;

    [Header("Data")]
    private FishData fishData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Hunger Logic")]
    [SerializeField] private float hungerMeterMax = 100f;
    private float currentHungerMeter = 100f;
    [SerializeField] private float hungerCooldown = 120f;
    private float currentHungerCooldown = 120f;
    [SerializeField] private bool isHungry = false;
    [SerializeField] private bool isHungryCooldown = false;

    [Header("Avoidance")]
    [SerializeField] private float avoidanceForce = 5f;
    [SerializeField] private float avoidanceRadius = 1.2f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Boundary Settings")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private bool useManualBounds = true;

    private Rigidbody2D rb;
    private bool isBouncingBack = false;
    private bool isScared = false;
    private Transform targetFood;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        currentMoveSpeed = Random.Range(minSpeed, maxSpeed);
        wanderDirection = Random.insideUnitCircle.normalized;
        currentHungerMeter = hungerMeterMax;
        currentHungerCooldown = hungerCooldown;
    }

    private void OnEnable()
    {
        gameEvent.OnScaringFish += ScaringFish;
    }

    private void OnDisable()
    {
        gameEvent.OnScaringFish -= ScaringFish;
    }

    void ScaringFish(Fish target)
    {
        if (target == this)
        {
            OnScared();
        }
    }

    void Update()
    {
        HandleHungerLogic();

        if (!isScared)
        {
            if (!isHungryCooldown && currentHungerMeter < hungerMeterMax)
            {
                ChangeState(FishState.SeekFood);
            }
            else
            {
                ChangeState(FishState.Wander);
            }
        }

        if (IsAtEdge())
        {
            isBouncingBack = true;
            wanderDirection = ((Vector2)Vector3.zero - (Vector2)transform.position).normalized;
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            MoveTowards(wanderDirection, randomSpeed);
        }
        else
        {
            isBouncingBack = false;

            switch (fishState)
            {
                case FishState.Wander:
                    WanderMovement();
                    break;
                case FishState.SeekFood:
                    SeekFoodMovement();
                    break;
                case FishState.RunAway:
                    RunAwayMovement();
                    break;
            }
        }

        ApplyMovement();
    }

    void WanderMovement()
    {
        if (currentAccelerateTime <= 0)
        {
            wanderDirection = (Random.insideUnitCircle + (Vector2)transform.right).normalized;
            currentMoveSpeed = Random.Range(minSpeed, maxSpeed);
            currentAccelerateTime = Random.Range(minAccelerateTime, maxAccelerateTime);
        }
        else
        {
            currentAccelerateTime -= Time.deltaTime;
        }

        MoveTowards(wanderDirection, currentMoveSpeed);
    }

    void SeekFoodMovement()
    {
        if (targetFood != null)
        {
            Vector2 direction = (targetFood.position - transform.position).normalized;
            MoveTowards(direction);

            if (Vector2.Distance(transform.position, targetFood.position) < 0.4f)
            {
                Eat(targetFood.gameObject);
            }
        }
        else
        {
            Collider2D foodCol = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Food"));
            if (foodCol != null)
            {
                targetFood = foodCol.transform;
            }
            else
            {
                WanderMovement();
            }
        }
    }

    void RunAwayMovement()
    {
        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));
        Vector2 finalPos = new Vector2(worldPos.x, worldPos.y);

        Vector2 runDir = ((Vector2)transform.position - finalPos).normalized;
        MoveTowards(runDir, maxSpeed * 1.5f);
    }

    void HandleHungerLogic()
    {
        if (isHungryCooldown)
        {
            currentHungerCooldown -= Time.deltaTime;
            if (currentHungerCooldown <= 0)
            {
                isHungryCooldown = false;
                currentHungerCooldown = hungerCooldown;
            }
        }
        else
        {
            if (currentHungerMeter > 0)
            {
                currentHungerMeter -= Time.deltaTime * 2f;
            }
            else
            {
                currentHungerMeter = 0;
            }
        }
    }

    void HungerCooldown()
    {
        currentHungerCooldown -= Time.deltaTime;
        if (currentHungerCooldown <= 0)
        {
            isHungryCooldown = false;
            isHungry = false;
            currentHungerCooldown = hungerCooldown;
        }
    }

    void Eat(GameObject food)
    {
        Food foodScript = food.GetComponent<Food>();
        if (foodScript != null)
        {
            ///// Lapar bertambah sesuai foodValue, tidak langsung jadi 100
            currentHungerMeter = Mathf.Min(currentHungerMeter + foodScript.FoodValue, hungerMeterMax);
        }

        food.gameObject.SetActive(false);
        targetFood = null;

        if (currentHungerMeter >= hungerMeterMax)
        {
            isHungryCooldown = true;
            currentHungerCooldown = hungerCooldown;
            ChangeState(FishState.Wander);
        }
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

    public void OnScared()
    {
        if (!isScared) StartCoroutine(ScareRoutine());
    }

    bool IsAtEdge()
    {
        if (useManualBounds)
        {
            return transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
                   transform.position.y < minBounds.y || transform.position.y > maxBounds.y;
        }
        else
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            return viewPos.x < 0.05f || viewPos.x > 0.95f || viewPos.y < 0.05f || viewPos.y > 0.95f;
        }
    }

    IEnumerator ScareRoutine()
    {
        isScared = true;
        ChangeState(FishState.RunAway);
        yield return new WaitForSeconds(2f);
        ChangeState(FishState.Wander);
        isScared = false;
    }

    public void ChangeState(FishState newState)
    {
        if (fishState == newState) return;
        fishState = newState;

        switch (fishState)
        {
            case FishState.Wander:
                
                break;
            case FishState.SeekFood:

                break;
            case FishState.RunAway:

                break;
            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }

    public void InitMetaData(float newDetectionRadius, float newHungerMeterMax, float newHungerCooldown, float newAvoidanceForce, float newAvoidanceRadius, Vector2 newMinBounds, Vector2 newMaxBounds)
    {
        detectionRadius = newDetectionRadius;
        hungerMeterMax = newHungerMeterMax;
        hungerCooldown = newHungerCooldown;
        avoidanceForce = newAvoidanceForce;
        avoidanceRadius = newAvoidanceRadius;
        minBounds = newMinBounds;
        maxBounds = newMaxBounds;
    }

    public void InitData(FishData newFishData)
    {
        fishData = newFishData;

        SpecsFishByType specs = FishManager.Instance.GetSpecsByType(fishData.fishType);

        if (specs != null)
        {
            minSpeed = specs.minSpeed;
            maxSpeed = specs.maxSpeed;
        }

        currentMoveSpeed = Random.Range(minSpeed, maxSpeed);
        spriteRenderer.sprite = fishData.fishSprite;
    }
}

[System.Serializable]
public enum FishState
{
    Wander,
    SeekFood,
    RunAway
}