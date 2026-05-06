using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public int slackValue = 10;
    public float sanityImpact = 5f;
    public float suspicionImpact = 0f;
    public GameObject interactIndicator;
    public AudioClip useSound;
    public string description;

    protected bool isUsed = false;
    protected float useCooldown = 0f;
    protected float cooldownDuration = 2f;

    protected virtual void Start()
    {
        if (interactIndicator != null)
            interactIndicator.SetActive(false);
    }

    void Update()
    {
        if (useCooldown > 0)
            useCooldown -= Time.deltaTime;

        CheckPlayerProximity();
    }

    void CheckPlayerProximity()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Player"));

        if (player != null && interactIndicator != null)
        {
            interactIndicator.SetActive(true);
        }
        else if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }
    }

    public virtual bool Interact(PlayerController player)
    {
        if (useCooldown > 0)
            return false;

        if (isUsed && !IsReusable())
            return false;

        // Perform interaction
        PerformInteraction(player);

        // Update cooldown
        useCooldown = cooldownDuration;

        return true;
    }

    protected virtual void PerformInteraction(PlayerController player)
    {
        player.AddSlackPoints(slackValue);
        player.UpdateSanity(sanityImpact);

        if (suspicionImpact > 0)
        {
            player.UpdateSuspicion(suspicionImpact);
        }

        // Play sound
        if (useSound != null)
        {
            AudioSource.PlayClipAtPoint(useSound, transform.position);
        }

        // Trigger animation
        TriggerInteractionAnimation();
    }

    protected virtual void TriggerInteractionAnimation()
    {
        // Override in derived classes
    }

    protected virtual bool IsReusable()
    {
        return true;
    }

    protected virtual string GetInteractionText()
    {
        return description;
    }
}
