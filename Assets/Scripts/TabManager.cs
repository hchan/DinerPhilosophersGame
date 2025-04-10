using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{
    public RectTransform instructionsPanel; // Assign your panel in the inspector
    public RectTransform diningTablePanel; // Assign your panel in the inspector
    public Button instructionsButton; // Assign your button in the inspector
    public Button diningTableButton; // Assign your button in the inspector


    public List<Button> tabButtons; // Assign your buttons in the inspector
    public List<RectTransform> tabPanels; // Assign your panels in the inspector

    public Button continueButton; // Assign your button in the inspector
    public void ShowTab(int index)
    {
      
    }

     void Start()
    {
        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(() => OnButtonClick(instructionsButton.name));  // Passing a label for the button
        }
        if (diningTableButton != null)
        {
            diningTableButton.onClick.AddListener(() => OnButtonClick(diningTableButton.name));  // Passing a label for the button
        }
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(() => OnButtonClick(continueButton.name));  // Passing a label for the button
        }
        tabButtons.Add(instructionsButton);
        tabButtons.Add(diningTableButton);

        tabPanels.Add(instructionsPanel);
        tabPanels.Add(diningTablePanel);
    }

    // General method to handle button click
    void OnButtonClick(string buttonName)
    {
        Debug.Log(buttonName + " Button clicked");
        // Handle interactions based on the button name
        switch (buttonName)
        {
            case "instructionsButton":
                for (int i = 0; i < tabButtons.Count; i++)
                {
                    SetButtonAlpha(tabButtons[i], 0.5f); // Set all buttons to 50% alpha
                }
                SetButtonAlpha(instructionsButton, 1f); // Set the clicked button to 100% alpha
                for (int i = 0; i < tabPanels.Count; i++)
                {
                    tabPanels[i].gameObject.SetActive(false); // Hide all panels
                }
                instructionsPanel.gameObject.SetActive(true); 
                break;
            case "diningTableButton":
            case "continueButton":
                for (int i = 0; i < tabButtons.Count; i++)
                {
                    SetButtonAlpha(tabButtons[i], 0.5f); // Set all buttons to 50% alpha
                }
                SetButtonAlpha(diningTableButton, 1f); // Set the clicked button to 100% alpha
                for (int i = 0; i < tabPanels.Count; i++)
                {
                    tabPanels[i].gameObject.SetActive(false); // Hide all panels
                }
                diningTablePanel.gameObject.SetActive(true);
                break;

            default:
                Debug.Log("Unknown button clicked: " + buttonName);
                break;
        }
    }

    void SetButtonAlpha(Button button, float alpha)
    {
        if (button != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color color = buttonImage.color;
                color.a = alpha; // Change the alpha
                buttonImage.color = color;
            }
        }
    }
}