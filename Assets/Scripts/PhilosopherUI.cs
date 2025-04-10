using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        gameManager.availablePickupChopsticksDropdown.ClearOptions();
        for (int i = 0; i < gameManager.chopstickData[philosopherId].availablePickupChopsticks.Count; i++)
        {
            Debug.Log("Adding option: " + gameManager.chopstickData[philosopherId].availablePickupChopsticks[i]);
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("chopstick" + 
            gameManager.chopstickData[philosopherId].availablePickupChopsticks[i]);
            gameManager.availablePickupChopsticksDropdown.options.Add(option);
        }
        
        if (gameManager.selectedPhilosopherId != philosopherId)
        {
           SetAlpha(gameManager.highlightAlpha);
        }
        
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (gameManager.selectedPhilosopherId != philosopherId)
        {
            SetAlpha(gameManager.defaultAlpha);
        }
    }


    public int GetLeftChopstickId() {
        return gameManager.GetLeftChopstickId(philosopherId);
    }

    public int GetRightChopstickId() {
        return gameManager.GetRightChopstickId(philosopherId);
    }
    void SetAlpha(float alpha)
    {
        gameManager.ResetAlpha();
        gameManager.SetAlpha(gameObject, alpha);
        gameManager.SetAlpha(gameManager.Get("chopstick" + GetLeftChopstickId()), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstickText" + GetLeftChopstickId()), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstick" + GetRightChopstickId()), alpha);
        gameManager.SetAlpha(gameManager.Get("chopstickText" + GetRightChopstickId()), alpha);
    }
}
