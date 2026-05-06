using UnityEngine;

// Alt-Tab Dash Mini-Game
public class AltTabDash : MonoBehaviour
{
    public float timeLimit = 5f;
    public int rewardPoints = 25;
    public GameObject dashIndicator;

    private bool isActive = false;
    private float timeRemaining;
    private PlayerController player;

    public void StartDash(PlayerController currentPlayer)
    {
        isActive = true;
        timeRemaining = timeLimit;
        player = currentPlayer;
    }

    void Update()
    {
        if (!isActive) return;

        timeRemaining -= Time.deltaTime;

        // Player must press Space to dodge
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Success();
        }

        if (timeRemaining <= 0)
        {
            Fail();
        }
    }

    void Success()
    {
        isActive = false;
        player.AddSlackPoints(rewardPoints);
        GameManager.Instance.TriggerEvent("AltTabSuccess");
    }

    void Fail()
    {
        isActive = false;
        player.UpdateSuspicion(20f);
        GameManager.Instance.TriggerEvent("AltTabFailed");
    }
}

// Excuse Delivery Slider
public class ExcuseDelivery : MonoBehaviour
{
    public float sweetSpotMin = 0.3f;
    public float sweetSpotMax = 0.7f;
    public float confidenceMultiplier = 2f;
    public int basePoints = 20;

    private bool isActive = false;
    private PlayerController player;
    private float sliderValue = 0f;

    public void StartDelivery(PlayerController currentPlayer)
    {
        isActive = true;
        player = currentPlayer;
    }

    void Update()
    {
        if (!isActive) return;

        // Slider controlled by player input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            sliderValue -= Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            sliderValue += Time.deltaTime;

        sliderValue = Mathf.Clamp(sliderValue, 0f, 1f);

        // Submit excuse
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EvaluateExcuse();
        }
    }

    void EvaluateExcuse()
    {
        isActive = false;

        if (sliderValue >= sweetSpotMin && sliderValue <= sweetSpotMax)
        {
            // Perfect delivery
            int points = Mathf.RoundToInt(basePoints * confidenceMultiplier);
            player.AddSlackPoints(points);
            GameManager.Instance.TriggerEvent("PerfectExcuse");
        }
        else if (sliderValue >= 0.2f && sliderValue <= 0.8f)
        {
            // Good delivery
            player.AddSlackPoints(basePoints);
            GameManager.Instance.TriggerEvent("GoodExcuse");
        }
        else
        {
            // Failed delivery
            player.UpdateSuspicion(15f);
            GameManager.Instance.TriggerEvent("BadExcuse");
        }
    }
}

// Sneak Past Security
public class SneakPastSecurity : MonoBehaviour
{
    public float guardLookDirection = 0f;
    public float timeToSneak = 2f;
    public int earlyExitBonus = 15;

    private bool isActive = false;
    private float sneakTimer = 0f;
    private PlayerController player;

    public void StartSneak(PlayerController currentPlayer)
    {
        isActive = true;
        sneakTimer = 0f;
        player = currentPlayer;
    }

    void Update()
    {
        if (!isActive) return;

        sneakTimer += Time.deltaTime;

        // Player must hold still to sneak
        if (Input.GetKey(KeyCode.C))
        {
            if (sneakTimer >= timeToSneak)
            {
                Success();
            }
        }
        else
        {
            Fail();
        }
    }

    void Success()
    {
        isActive = false;
        player.AddSlackPoints(earlyExitBonus);
        GameManager.Instance.TriggerEvent("SneakSuccess");
    }

    void Fail()
    {
        isActive = false;
        player.UpdateSuspicion(10f);
        GameManager.Instance.TriggerEvent("SneakFailed");
    }
}

// Bluff Builder (End of Day Review)
public class BluffBuilder : MonoBehaviour
{
    public GameObject[] fakeAccomplishments;
    public GameObject reviewSummary;
    public float timeLimit = 15f;
    public int rewardPoints = 50;

    private bool isActive = false;
    private float timeRemaining;
    private int accomplishmentsPlaced = 0;

    public void StartReview(PlayerController player)
    {
        isActive = true;
        timeRemaining = timeLimit;
        accomplishmentsPlaced = 0;
    }

    void Update()
    {
        if (!isActive) return;

        timeRemaining -= Time.deltaTime;

        // Player drags and drops accomplishments
        // Check for drop events
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Check if dropped on review summary
            if (reviewSummary != null)
            {
                accomplishmentsPlaced++;

                if (accomplishmentsPlaced >= fakeAccomplishments.Length)
                {
                    CompleteReview();
                }
            }
        }

        if (timeRemaining <= 0)
        {
            FailReview();
        }
    }

    void CompleteReview()
    {
        isActive = false;
        GameManager.Instance.TriggerEvent("ReviewComplete");
    }

    void FailReview()
    {
        isActive = false;
        GameManager.Instance.TriggerEvent("ReviewFailed");
    }
}

// Inbox Avalanche
public class InboxAvalanche : MonoBehaviour
{
    public int totalEmails = 20;
    public float emailSpawnRate = 0.5f;
    public GameObject emailPrefab;
    public float timeLimit = 10f;
    public int rewardPoints = 30;

    private bool isActive = false;
    private float spawnTimer = 0f;
    private int emailsMuted = 0;
    private int emailsSpawned = 0;

    public void StartAvalanche(PlayerController player)
    {
        isActive = true;
        spawnTimer = 0f;
        emailsMuted = 0;
        emailsSpawned = 0;
    }

    void Update()
    {
        if (!isActive) return;

        // Spawn emails
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= emailSpawnRate && emailsSpawned < totalEmails)
        {
            SpawnEmail();
            spawnTimer = 0f;
            emailsSpawned++;
        }

        // Check for mute clicks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            emailsMuted++;
        }

        if (emailsMuted >= totalEmails || emailsSpawned >= totalEmails)
        {
            Complete();
        }
    }

    void SpawnEmail()
    {
        if (emailPrefab != null)
        {
            Instantiate(emailPrefab, transform.position, Quaternion.identity);
        }
    }

    void Complete()
    {
        isActive = false;
        GameManager.Instance.TriggerEvent("InboxCleared");
    }
}
