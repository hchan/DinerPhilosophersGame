using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{


    public Button continueButton; // Assign your button in the inspector
    public void ShowTab(int index)
    {

    }

    void Start()
    {
        for (int i = 0; i < GameManager.Instance.tabButtons.Count; i++)
        {
            Button button = GameManager.Instance.tabButtons[i];
            button.onClick.AddListener(() => OnButtonClick(button));  // Passing a label for the button
        }
        continueButton.onClick.AddListener(() => ContinueButtonClick(continueButton)); // Assign the continue button click event
    }

    void ContinueButtonClick(Button continueButton)
    {
        Debug.Log("Continue Button clicked");
        SetButtonsAlpha(0.5f); // Set all buttons to 50% alpha
        HideAllPanels(); // Hide all panels
        RectTransform panel = GameManager.Instance.Get("diningTablePanel").GetComponent<RectTransform>();
        panel.gameObject.SetActive(true); // Show the dining table panel
        SetButtonAlpha(continueButton, 1f); // Set the clicked button to 100% alpha 
    }

    // General method to handle button click
    void OnButtonClick(Button button)
    {
        string buttonName = button.name; // Get the name of the clicked button
        Debug.Log(buttonName + " Button clicked");
        SetButtonsAlpha(0.5f); // Set all buttons to 50% alpha
        HideAllPanels(); // Hide all panels
        RectTransform panel = GameManager.Instance.Get(buttonName.Replace("Button", "") + "Panel").GetComponent<RectTransform>();
        panel.gameObject.SetActive(true); // Show the corresponding panel
        SetButtonAlpha(button, 1f); // Set the clicked button to 100% alpha
    }

    void SetButtonsAlpha(float alpha)
    {
        foreach (Button button in GameManager.Instance.tabButtons)
        {
            SetButtonAlpha(button, alpha);
        }
    }

    void HideAllPanels()
    {
        foreach (RectTransform panel in GameManager.Instance.tabPanels)
        {
            panel.gameObject.SetActive(false); // Hide all panels
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