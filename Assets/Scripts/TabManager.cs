using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TabManager : MonoBehaviour
{


    public Button continueButton; // Assign your button in the inspector
    public void ShowTab(string name)
    {
        Debug.Log("Continue Button clicked");
        SetButtonsAlpha(0.5f); // Set all buttons to 50% alpha
        HideAllPanels(); // Hide all panels
        RectTransform panel = GameManager.Instance.Get($"{name}Panel").GetComponent<RectTransform>();
        panel.gameObject.SetActive(true); // Show the dining table panel
        SetButtonAlpha(GameManager.Instance.Get($"{name}Button").GetComponent<Button>(), 1f); // Set the clicked button to 100% alpha 
    }

    void Start()
    {
        for (int i = 0; i < GameManager.Instance.tabButtons.Count; i++)
        {
            Button button = GameManager.Instance.tabButtons[i];
            string buttonName = button.name; // Get the name of the button
            if (buttonName == "instructionsButton") {
                SetButtonAlpha(button, 1f); // Set all buttons to 50% alpha
            } else {
                SetButtonAlpha(button, 0.5f); // Set all buttons to 50% alpha
            }
            button.onClick.AddListener(() => OnButtonClick(button));  // Passing a label for the button
        }
        for (int i = 0; i < GameManager.Instance.tabPanels.Count; i++) {
            string panelName = GameManager.Instance.tabPanels[i].name;
            if (panelName == "instructionsPanel") {
                GameManager.Instance.tabPanels[i].gameObject.SetActive(true); // Show the first panel by default
            } else {
                GameManager.Instance.tabPanels[i].gameObject.SetActive(false); // Hide all other panels
            }
        }
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(() => ContinueButtonClick(continueButton)); // Assign the continue button click event
        }
    }

    void ContinueButtonClick(Button continueButton)
    {
        Debug.Log("Continue Button clicked");
        SetButtonsAlpha(0.5f); // Set all buttons to 50% alpha
        HideAllPanels(); // Hide all panels
        RectTransform panel = GameManager.Instance.Get("diningTablePanel").GetComponent<RectTransform>();
        panel.gameObject.SetActive(true); // Show the dining table panel
        SetButtonAlpha(GameManager.Instance.Get("diningTableButton").GetComponent<Button>(), 1f); // Set the clicked button to 100% alpha 
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