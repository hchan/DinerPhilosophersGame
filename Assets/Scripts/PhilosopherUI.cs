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
        
        /*
        gameManager.Get("instructions1").SetActive(false);
        gameManager.Get("pickupChopsticksText").SetActive(true);
        gameManager.Get("availablePickupChopsticksDropdown").SetActive(true);
        */
    }



    // Example button click handler
    void OnChopstickButtonClick(int index)
    {
        Debug.Log("Chopstick button clicked at index: " + index);
        // Handle the button click logic (e.g., pick up chopsticks)
    }

    void OnMouseEnter()
    {
        // Set the cursor to the hoverCursor
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);

        // Get the availableChopsticksSelector GameObject
        GameObject availableChopsticksSelector = gameManager.Get("availableChopsticksSelector");
 
        // Check that the availableChopsticksSelector is not null
        if (availableChopsticksSelector == null)
        {
            Debug.LogError("availableChopsticksSelector not found!");
            return;
        }
        gameManager.Get("philosopherSelectedText").GetComponent<TMP_Text>().text = 
            gameManager.philosopherNames[philosopherId];
        // Loop through each available chopstick in the data
        for (int i = 0; i < gameManager.chopstickData[philosopherId].availablePickupChopsticks.Count; i++)
        {
            Debug.Log("Adding option: " + gameManager.chopstickData[philosopherId].availablePickupChopsticks[i]);

            GameObject buttonObj = gameManager.Get("chopstickAvailable" + i);
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();

            label.text = "chopstick" + gameManager.chopstickData[philosopherId].availablePickupChopsticks[i];

/*
            // Set the button as a child of availableChopsticksSelector (use Transform for proper hierarchy management)
            buttonObj.transform.SetParent(availableChopsticksSelector.transform, true); // Use 'false' to keep local position

            buttonObj = gameManager.Get("chopstickAvailable1");
            label = buttonObj.GetComponentInChildren<TMP_Text>();

            label.text = "chopstick" + gameManager.chopstickData[philosopherId].availablePickupChopsticks[i];
*/

            // Set the button as a child of availableChopsticksSelector (use Transform for proper hierarchy management)
            buttonObj.transform.SetParent(availableChopsticksSelector.transform, false); // Use 'false' to keep local position


        }

        // If selected philosopher is not the current one, set alpha to highlight
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
