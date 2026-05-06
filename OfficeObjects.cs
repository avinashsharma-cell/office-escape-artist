using UnityEngine;

// Desk Object
public class OfficeDesk : InteractableObject
{
    public GameObject paperStack;
    public GameObject computerScreen;
    public float workScreenTime = 5f;
    private bool isWorking = false;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);
        isWorking = true;

        // Toggle between work screen and personal browsing
        if (computerScreen != null)
        {
            computerScreen.SetActive(!computerScreen.activeSelf);
        }

        StartCoroutine(WorkCycle());
    }

    System.Collections.IEnumerator WorkCycle()
    {
        yield return new WaitForSeconds(workScreenTime);
        isWorking = false;
    }
}

// Coffee Machine
public class CoffeeMachine : InteractableObject
{
    public GameObject coffeeCup;
    public float brewingTime = 3f;
    private bool isBrewing = false;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (!isBrewing)
        {
            isBrewing = true;
            StartCoroutine(BrewCoffee());
        }
    }

    System.Collections.IEnumerator BrewCoffee()
    {
        yield return new WaitForSeconds(brewingTime);
        isBrewing = false;

        if (coffeeCup != null)
            coffeeCup.SetActive(true);
    }
}

// Water Cooler
public class WaterCooler : InteractableObject
{
    public GameObject gossipBubble;
    public float gossipDuration = 5f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (gossipBubble != null)
            gossipBubble.SetActive(true);

        StartCoroutine(GossipSession());
    }

    System.Collections.IEnumerator GossipSession()
    {
        yield return new WaitForSeconds(gossipDuration);
        if (gossipBubble != null)
            gossipBubble.SetActive(false);
    }
}

// Printer
public class OfficePrinter : InteractableObject
{
    public GameObject printedDocument;
    public float printTime = 4f;
    private bool isPrinting = false;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (!isPrinting)
        {
            isPrinting = true;
            suspicionImpact = 15f; // Risky activity

            StartCoroutine(PrintDocument());
        }
    }

    System.Collections.IEnumerator PrintDocument()
    {
        yield return new WaitForSeconds(printTime);
        isPrinting = false;

        if (printedDocument != null)
            printedDocument.SetActive(true);
    }
}

// Vending Machine
public class VendingMachine : InteractableObject
{
    public GameObject[] snacks;
    public float purchaseTime = 2f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        int randomSnack = Random.Range(0, snacks.Length);
        if (snacks[randomSnack] != null)
            snacks[randomSnack].SetActive(true);

        suspicionImpact = 5f;
    }
}

// Office Plant
public class OfficePlant : InteractableObject
{
    public GameObject waterDroplet;
    public float waterTime = 2f;
    private int waterCount = 0;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);
        waterCount++;

        if (waterDroplet != null)
            waterDroplet.SetActive(true);

        StartCoroutine(WaterPlant());
    }

    System.Collections.IEnumerator WaterPlant()
    {
        yield return new WaitForSeconds(waterTime);
        if (waterDroplet != null)
            waterDroplet.SetActive(false);
    }
}

// Sofa Corner (Nap Zone)
public class SofaCorner : InteractableObject
{
    public float napDuration = 10f;
    public float sanityRecovery = 30f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        slackValue = 70;
        sanityImpact = sanityRecovery;

        StartCoroutine(Nap());
    }

    System.Collections.IEnumerator Nap()
    {
        yield return new WaitForSeconds(napDuration);
    }
}

// Manager's Chair (High Risk)
public class ManagersChair : InteractableObject
{
    public GameObject powerMoveBadge;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        slackValue = 50;
        suspicionImpact = 25f; // High risk

        if (powerMoveBadge != null)
            powerMoveBadge.SetActive(true);
    }

    protected override bool IsReusable()
    {
        return false; // One-time stunt
    }
}

// Manager's Coffee Mug
public class ManagersCoffeeMug : InteractableObject
{
    public GameObject legendaryBadge;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        slackValue = 80;
        suspicionImpact = 40f; // Very high risk

        if (legendaryBadge != null)
            legendaryBadge.SetActive(true);
    }

    protected override bool IsReusable()
    {
        return false; // One-time legendary stunt
    }
}

// Whiteboard (Catastrophic if caught)
public class Whiteboard : InteractableObject
{
    public GameObject erasedChecklist;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        slackValue = 100;
        suspicionImpact = 50f; // Catastrophic

        if (erasedChecklist != null)
            erasedChecklist.SetActive(true);
    }
}

// Sticky Notes (Stationery Theft)
public class StickyNotes : InteractableObject
{
    private int noteCount = 0;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);
        noteCount++;

        if (noteCount >= 5)
        {
            // Trigger event: Stationery theft discovered
            GameManager.Instance.TriggerEvent("StationeryTheft");
        }
    }
}

// Desk Phone
public class DeskPhone : InteractableObject
{
    public GameObject callBubble;
    public float callDuration = 3f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (callBubble != null)
            callBubble.SetActive(true);

        StartCoroutine(FakeCall());
    }

    System.Collections.IEnumerator FakeCall()
    {
        yield return new WaitForSeconds(callDuration);
        if (callBubble != null)
            callBubble.SetActive(false);
    }
}

// Recycling Bin
public class RecyclingBin : InteractableObject
{
    public GameObject sortedItems;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (sortedItems != null)
            sortedItems.SetActive(true);
    }
}

// Notice Board
public class NoticeBoard : InteractableObject
{
    public GameObject fakeNotes;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (fakeNotes != null)
            fakeNotes.SetActive(true);
    }
}

// Stationery Cupboard
public class StationeryCupboard : InteractableObject
{
    public GameObject[] items;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        int randomItem = Random.Range(0, items.Length);
        if (items[randomItem] != null)
            items[randomItem].SetActive(true);
    }
}

// Toilet Cubicle (Timed Safe Zone)
public class ToiletCubicle : InteractableObject
{
    public float safeZoneDuration = 30f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        player.UpdateSanity(20f);
        suspicionImpact = 0f; // Safe zone

        StartCoroutine(HideInCubicle());
    }

    System.Collections.IEnumerator HideInCubicle()
    {
        yield return new WaitForSeconds(safeZoneDuration);
    }
}

// Microwave
public class Microwave : InteractableObject
{
    public GameObject heatedFood;
    public float heatingTime = 3f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        StartCoroutine(HeatFood());
    }

    System.Collections.IEnumerator HeatFood()
    {
        yield return new WaitForSeconds(heatingTime);
        if (heatedFood != null)
            heatedFood.SetActive(true);
    }
}

// TV/Screen
public class OfficeTV : InteractableObject
{
    public GameObject newsScreen;
    public float watchTime = 5f;

    protected override void PerformInteraction(PlayerController player)
    {
        base.PerformInteraction(player);

        if (newsScreen != null)
            newsScreen.SetActive(true);

        StartCoroutine(WatchNews());
    }

    System.Collections.IEnumerator WatchNews()
    {
        yield return new WaitForSeconds(watchTime);
        if (newsScreen != null)
            newsScreen.SetActive(false);
    }
}
