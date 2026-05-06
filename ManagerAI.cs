using UnityEngine;

public class ManagerAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float detectionRadius = 5f;
    public float suspicionRadius = 8f;
    public Transform[] patrolPoints;
    public GameObject detectionCone;

    private int currentPatrolPoint = 0;
    private bool playerDetected = false;
    private bool isInvestigating = false;
    private float investigationTimer = 0f;
    private float investigationDuration = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        detectionCone.SetActive(false);
    }

    void Update()
    {
        if (!isInvestigating)
        {
            Patrol();
        }
        else
        {
            Investigation();
        }

        CheckDetection();
        UpdateDetectionVisuals();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolPoint];
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            patrolSpeed * Time.deltaTime
        );

        // Face direction of movement
        if (targetPoint.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        // Check if reached patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        // Update animation
        float distanceToTarget = Vector2.Distance(transform.position, targetPoint.position);
        animator.SetFloat("Speed", distanceToTarget > 0.5f ? patrolSpeed : 0f);
    }

    void CheckDetection()
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var col in overlaps)
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                // Check if player is slacking
                if (player.isSlacking && !player.isHidden)
                {
                    DetectPlayer(player);
                    return;
                }

                // Check if player is in restricted area
                if (IsPlayerInRestrictedArea(player))
                {
                    DetectPlayer(player);
                    return;
                }
            }
        }
    }

    void DetectPlayer(PlayerController player)
    {
        if (!playerDetected)
        {
            playerDetected = true;
            isInvestigating = true;
            investigationTimer = investigationDuration;
            animator.SetTrigger("DetectPlayer");

            // Notify game manager
            GameManager.Instance.OnManagerDetection(player);
        }

        // Increase suspicion
        player.UpdateSuspicion(Time.deltaTime * 5f);
    }

    void Investigation()
    {
        investigationTimer -= Time.deltaTime;

        // Look around animation
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Investigating", true);

        if (investigationTimer <= 0)
        {
            isInvestigating = false;
            playerDetected = false;
            animator.SetBool("Investigating", false);

            // Check if player is still in detection range
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, suspicionRadius);
            foreach (var col in overlaps)
            {
                PlayerController player = col.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.UpdateSuspicion(Time.deltaTime * 2f);
                }
            }
        }
    }

    bool IsPlayerInRestrictedArea(PlayerController player)
    {
        // Check if player is near manager's cabin
        float distanceToCabin = Vector2.Distance(transform.position, GameManager.Instance.managerCabinPosition);
        return distanceToCabin < 3f;
    }

    void UpdateDetectionVisuals()
    {
        if (playerDetected || isInvestigating)
        {
            detectionCone.SetActive(true);
        }
        else
        {
            detectionCone.SetActive(false);
        }
    }

    public void FreezeForDuration(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    System.Collections.IEnumerator FreezeCoroutine(float duration)
    {
        patrolSpeed = 0f;
        detectionCone.SetActive(false);

        yield return new WaitForSeconds(duration);

        patrolSpeed = 2f;
    }
}
