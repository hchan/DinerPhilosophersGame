using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void ScrollNow()
    {
        StartCoroutine(ScrollAfterLayout());
    }

    IEnumerator ScrollAfterLayout()
    {
        // Wait a couple of frames for WebGL layout delay
        yield return null;
        yield return null;

        // Force layout rebuild just to be safe
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        // Set to bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }

    void LateUpdate()
    {
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
