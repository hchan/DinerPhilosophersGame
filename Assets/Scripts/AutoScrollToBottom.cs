using UnityEngine;
using UnityEngine.UI;

public class AutoScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;

    void LateUpdate()
    {
        // Scroll all the way to the bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
