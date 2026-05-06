public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float runMultiplier = 1.5f;
    public int slackPoints = 0;
    public float sanity = 100f;
    public float suspicion = 0f;
    public int currentFloor = 1;
    public int maxFloor = 5;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    public bool isSlacking = false;
    public bool isHidden = false;
    public bool isSneaking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        // Lock cursor to window
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        HandleMovement();
        HandleActions();
        HandleCrouch();
        HandleFloorTransition();
        UpdateCamera();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveX = 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveY = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveY = -1f;

        // Check if running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = moveSpeed * (isRunning ? runMultiplier : 1f);

        // Apply movement
        Vector2 velocity = new Vector2(moveX, moveY).normalized * speed;
        rb.velocity = velocity;

        // Update animations
        animator.SetFloat("Speed", velocity.magnitude);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsSlacking", isSlacking);

        // Update suspicion if running
        if (isRunning && !isHidden)
        {
            suspicion += Time.deltaTime * 2f;
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C) && isSneaking)
        {
            isHidden = !isHidden;
            animator.SetBool("IsCrouching", isHidden);
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformSlackAction();
        }
    }

    void TryInteract()
    {
        // Check for nearby interactable objects
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach (var col in nearbyObjects)
        {
            InteractableObject obj = col.GetComponent<InteractableObject>();
            if (obj != null && Vector2.Distance(transform.position, obj.transform.position) < 1.5f)
            {
                obj.Interact(this);
                break;
            }
        }
    }

    void PerformSlackAction()
    {
        if (!isSlacking)
        {
            isSlacking = true;
            animator.SetTrigger("StartSlacking");

            // Add passive slack points
            slackPoints += 5;
            sanity -= 2f;

            // Start coroutine for slacking duration
            StartCoroutine(StopSlackingAfterDuration());
        }
    }

    IEnumerator StopSlackingAfterDuration()
    {
        yield return new WaitForSeconds(3f);
        isSlacking = false;
        animator.SetTrigger("StopSlacking");
    }

    void HandleFloorTransition()
    {
        // Check if player reached elevator or stairs
        if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.E))
        {
            TransitionFloors();
        }
    }

    void TransitionFloors()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (currentFloor < maxFloor)
            {
                currentFloor++;
                LoadFloor(currentFloor);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (currentFloor > 1)
            {
                currentFloor--;
                LoadFloor(currentFloor);
            }
        }
    }

    void LoadFloor(int floorNumber)
    {
        // Load floor scene
        GameManager.Instance.ChangeFloor(floorNumber);
        transform.position = GameManager.Instance.GetFloorSpawnPoint(floorNumber);
    }

    void UpdateCamera()
    {
        // Smooth camera follow
        Vector3 targetPosition = transform.position;
        targetPosition.z = mainCamera.transform.position.z;
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetPosition,
            Time.deltaTime * 5f
        );
    }

    public void AddSlackPoints(int points)
    {
        slackPoints += points;
        GameManager.Instance.UpdateSlackPoints(slackPoints);
    }

    public void UpdateSanity(float change)
    {
        sanity = Mathf.Clamp(sanity + change, 0f, 100f);
        GameManager.Instance.UpdateSanity(sanity);
    }

    public void UpdateSuspicion(float change)
    {
        suspicion = Mathf.Clamp(suspicion + change, 0f, 100f);
        GameManager.Instance.UpdateSuspicion(suspicion);
    }

    public void HandleDetection()
    {
        suspicion += 15f;
        UpdateSuspicion(suspicion);
        animator.SetTrigger("Detected");
    }

    public void UnlockSneaking()
    {
        isSneaking = true;
    }
}
