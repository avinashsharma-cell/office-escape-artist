using UnityEngine;

// The Manager
public class ManagerNPC : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public Transform[] patrolPoints;
    public GameObject exclamationMark;
    public AudioClip managerFootsteps;

    private int currentPatrolPoint = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        exclamationMark.SetActive(false);
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.5f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        // Face movement direction
        if (target.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    public void ShowExclamation()
    {
        exclamationMark.SetActive(true);
    }

    public void HideExclamation()
    {
        exclamationMark.SetActive(false);
    }
}

// The HR Rep
public class HRRep : MonoBehaviour
{
    public GameObject warningBubble;
    public float rareSpawnChance = 0.1f;
    public float interactionDuration = 5f;

    private Animator animator;
    private bool isWarningActive = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        warningBubble.SetActive(false);
    }

    void Update()
    {
        // Random rare spawn check
        if (Random.value < rareSpawnChance * Time.deltaTime)
        {
            TriggerWarningEvent();
        }
    }

    public void TriggerWarningEvent()
    {
        isWarningActive = true;
        warningBubble.SetActive(true);

        StartCoroutine(WarningDuration());
    }

    System.Collections.IEnumerator WarningDuration()
    {
        yield return new WaitForSeconds(interactionDuration);
        isWarningActive = false;
        warningBubble.SetActive(false);
    }
}

// The Overachiever Colleague
public class OverachieverColleague : MonoBehaviour
{
    public float typingSpeed = 3f;
    public GameObject multipleMonitors;
    public float suspicionReductionAmount = 10f;

    private Animator animator;
    private bool isBefriended = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Typing animation
        animator.SetFloat("Speed", typingSpeed);
    }

    public void Befriend(PlayerController player)
    {
        isBefriended = true;

        // Reduce suspicion
        player.UpdateSuspicion(-suspicionReductionAmount);

        // Show friendly interaction
        GameManager.Instance.TriggerEvent("BefriendedOverachiever");
    }
}

// The Gossip
public class GossipNPC : MonoBehaviour
{
    public float gossipDuration = 8f;
    public GameObject gossipBubble;
    public int SPBoostAmount = 15;

    private Animator animator;
    private bool isGossiping = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        gossipBubble.SetActive(false);
    }

    void Update()
    {
        // Idle lean animation
        animator.SetFloat("Speed", 0f);
    }

    public void StartGossipSession(PlayerController player)
    {
        isGossiping = true;
        gossipBubble.SetActive(true);

        player.AddSlackPoints(SPBoostAmount);

        StartCoroutine(Gossip());
    }

    System.Collections.IEnumerator Gossip()
    {
        yield return new WaitForSeconds(gossipDuration);
        isGossiping = false;
        gossipBubble.SetActive(false);
    }
}

// The IT Guy
public class ITGuy : MonoBehaviour
{
    public float bribeCost = 30;
    public float fakeOutageDuration = 60f;
    public GameObject serverOutageIndicator;

    private Animator animator;
    private bool isBribed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        serverOutageIndicator.SetActive(false);
    }

    void Update()
    {
        // Confused idle animation
        animator.SetFloat("Speed", 0f);
    }

    public bool TryBribe(PlayerController player)
    {
        if (player.slackPoints >= bribeCost && !isBribed)
        {
            player.slackPoints -= bribeCost;
            isBribed = true;

            // Trigger fake server outage
            GameManager.Instance.TriggerEvent("ServerOutage");
            StartCoroutine(FakeOutage());

            return true;
        }
        return false;
    }

    System.Collections.IEnumerator FakeOutage()
    {
        serverOutageIndicator.SetActive(true);

        yield return new WaitForSeconds(fakeOutageDuration);

        serverOutageIndicator.SetActive(false);
        isBribed = false;
    }
}

// The Intern
public class InternNPC : MonoBehaviour
{
    public float nervousEnergy = 5f;
    public int workAssignmentCost = 20;
    public float relationshipDecayRate = 0.5f;

    private Animator animator;
    private float relationshipLevel = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nervous idle animation
        animator.SetFloat("Speed", nervousEnergy);

        // Decay relationship
        if (relationshipLevel > 0)
        {
            relationshipLevel -= relationshipDecayRate * Time.deltaTime;
        }
    }

    public void AssignWork(PlayerController player)
    {
        if (relationshipLevel > 20f)
        {
            player.AddSlackPoints(workAssignmentCost);
            relationshipLevel -= 30f;

            GameManager.Instance.TriggerEvent("AssignedInternWork");
        }
    }
}

// The Security Guard
public class SecurityGuard : MonoBehaviour
{
    public float patrolSpeed = 1f;
    public Transform[] patrolPoints;
    public float SPBoostAmount = 10f;
    public float chatDuration = 5f;

    private int currentPatrolPoint = 0;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolPoint];
        transform.position = Vector2.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.5f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        animator.SetFloat("Speed", patrolSpeed);
    }

    public void ChatWith(PlayerController player)
    {
        player.AddSlackPoints(SPBoostAmount);

        StartCoroutine(Chat());
    }

    System.Collections.IEnumerator Chat()
    {
        yield return new WaitForSeconds(chatDuration);
    }
}
