using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Button runButton; // Reference to the Run button

    void LateUpdate()
    {
        if (runButton.interactable == false)
        {
            // Scroll to the bottom of the scrollRect
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
