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
        gameManager.Get("availablePickupChopsticksDropdown").SetActive(true);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        GameObject availableChopsticksSelector = gameManager.Get("availableChopsticksSelector");
        for (int i = 0; i < gameManager.chopstickData[philosopherId].availablePickupChopsticks.Count; i++)
        {
            Debug.Log("Adding option: " + gameManager.chopstickData[philosopherId].availablePickupChopsticks[i]);
            // Create the button GameObject
            GameObject buttonObj = new GameObject("TextMeshProButton");

            // Set the button as a child of the vertical layout container
            buttonObj.transform.SetParent(availableChopsticksSelector.transform);

            // Add a RectTransform to the button
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30); // Set the size (Width, Height)

            // Add the TextMeshProUGUI component for button text
            TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();
            buttonText.text = "New Button";
            buttonText.fontSize = 18;

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


    public int GetLeftChopstickId()
    {
        return gameManager.GetLeftChopstickId(philosopherId);
    }

    public int GetRightChopstickId()
    {
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
