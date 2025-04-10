using UnityEngine;

public class PhilosopherUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Texture2D hoverCursor;

    private GameManager gameManager;

    private int leftChopstickId;
    private int rightChopstickId;

    private int philosopherId;
    void Start()
    {
        // Access the GameManager singleton
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        string name = gameObject.name;
        gameObject.name.Substring(name.Length - 1, 1);
        philosopherId = int.Parse(name.Substring(name.Length - 1, 1));

        leftChopstickId =  (philosopherId+1) % gameManager.numPhilosophers;
        rightChopstickId = philosopherId;
    }

    void OnMouseDown()
    {
        gameManager.selectedPhilosopherId = philosopherId;
        SetAlpha(gameManager.selectedAlpha);
        gameManager.Get("instructions1").SetActive(false);
        gameManager.Get("pickupChopsticksText").SetActive(true);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        if (gameManager.selectedPhilosopherId == philosopherId)
        {
           return;
        }
        SetAlpha(gameManager.highlightAlpha);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (gameManager.selectedPhilosopherId != philosopherId)
        {
            SetAlpha(gameManager.defaultAlpha);
        }
    }

    void SetAlpha(float alpha)
    {
        gameManager.ResetAlpha();
        gameManager.SetAlpha(gameObject, alpha);
        gameManager.SetAlpha(gameManager.Get("chopstick" + leftChopstickId), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstickText" + leftChopstickId), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstick" + rightChopstickId), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstickText" + rightChopstickId), alpha);
    }
}
