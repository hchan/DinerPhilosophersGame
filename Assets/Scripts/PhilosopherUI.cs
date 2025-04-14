using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PhilosopherUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Texture2D hoverCursor;

    private GameManager gameManager;


    private int philosopherId;
    void Start()
    {
        // Access the GameManager singleton
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        string name = gameObject.name;
        gameObject.name.Substring(name.Length - 1, 1);
        // example: "philosopher0" => philosopherId = 0
        philosopherId = int.Parse(name.Substring(name.Length - 1, 1));
    }

    void OnMouseDown()
    {
        // Set the cursor to the hoverCursor
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        gameManager.selectedPhilosopherId = philosopherId;
        // Tigress, Monkey, Viper, Crane, Mantis
        gameManager.Get("philosopherSelectedText").GetComponent<TMP_Text>().text =
            gameManager.philosopherNames[philosopherId] + " : philosopher" + philosopherId;
        ResetChopsticksUI();
        SetAlpha(gameManager.fullAlpha);
    }


    void OnMouseEnter()
    {
        gameManager.SetAlpha(gameObject, gameManager.highlightAlpha);
    }

    void ResetChopsticksUI()
    {
        GameObject availableChopsticksPickupSelector = gameManager.Get("availableChopsticksPickupSelector");
        GameObject availableChopsticksDropSelector = gameManager.Get("availableChopsticksDropSelector");
        GameObject orderChopsticksPickupSelector = gameManager.Get("orderChopsticksPickupSelector");
        GameObject orderChopsticksDropSelector = gameManager.Get("orderChopsticksDropSelector");
        GameObject chopsticksHolder = gameManager.Get("chopsticksHolder");
        // reset chopsticksHolder
        foreach (GameObject obj in gameManager.chopsticksInChopsticksHolder)
        {
            obj.transform.SetParent(chopsticksHolder.transform, false); // Use 'false' to keep local position 
        }
        // Loop through each available chopstick in the data
        // move from the chopsticksHolder to the appropriate selector
        for (int i = 0; i < gameManager.chopstickData[philosopherId].availablePickupChopsticks.Count; i++)
        {
            int chopstickId = gameManager.chopstickData[philosopherId].availablePickupChopsticks[i];
            Button buttonObj = gameManager.Get("availableChopstickPickup" + i).GetComponent<Button>();
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            label.text = "chopstick" + chopstickId;
            buttonObj.transform.SetParent(availableChopsticksPickupSelector.transform, false); // Use 'false' to keep local position
            buttonObj.onClick.RemoveAllListeners(); // Remove all listeners before adding a new one
            buttonObj.onClick.AddListener(() =>
            {
                gameManager.chopstickData[philosopherId].ConsumeAvailablePickupChopstick(chopstickId);
                ResetChopsticksUI();
            }
            );
        }
        for (int i = 0; i < gameManager.chopstickData[philosopherId].availableDropChopsticks.Count; i++)
        {
            int chopstickId = gameManager.chopstickData[philosopherId].availableDropChopsticks[i];
            Button buttonObj = gameManager.Get("availableChopstickDrop" + i).GetComponent<Button>();
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            label.text = "chopstick" + chopstickId;
            buttonObj.transform.SetParent(availableChopsticksDropSelector.transform, false); // Use 'false' to keep local position
            buttonObj.onClick.RemoveAllListeners(); // Remove all listeners before adding a new one
            buttonObj.onClick.AddListener(() =>
            {
                gameManager.chopstickData[philosopherId].ConsumeAvailableDropChopstick(chopstickId);
                ResetChopsticksUI();
            }
            );
        }

        for (int i = 0; i < gameManager.chopstickData[philosopherId].orderPickupChopsticks.Count; i++)
        {
            int chopstickId = gameManager.chopstickData[philosopherId].orderPickupChopsticks[i];
            Button buttonObj = gameManager.Get("orderChopstickPickup" + i).GetComponent<Button>();
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            label.text = "chopstick" + chopstickId;
            buttonObj.transform.SetParent(orderChopsticksPickupSelector.transform, false); // Use 'false' to keep local position
            buttonObj.onClick.RemoveAllListeners(); // Remove all listeners before adding a new one
            buttonObj.onClick.AddListener(() =>
            {
                gameManager.chopstickData[philosopherId].ConsumeOrderPickupChopstick(chopstickId);
                ResetChopsticksUI();
            }
            );
        }
        for (int i = 0; i < gameManager.chopstickData[philosopherId].orderDropChopsticks.Count; i++)
        {
            int chopstickId = gameManager.chopstickData[philosopherId].orderDropChopsticks[i];
            Button buttonObj = gameManager.Get("orderChopstickDrop" + i).GetComponent<Button>();
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            label.text = "chopstick" + chopstickId;
            buttonObj.transform.SetParent(orderChopsticksDropSelector.transform, false); // Use 'false' to keep local position
            buttonObj.onClick.RemoveAllListeners(); // Remove all listeners before adding a new one
            buttonObj.onClick.AddListener(() =>
            {
                gameManager.chopstickData[philosopherId].ConsumeOrderDropChopstick(chopstickId);
                ResetChopsticksUI();
            }
            );
        }
    }


    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
