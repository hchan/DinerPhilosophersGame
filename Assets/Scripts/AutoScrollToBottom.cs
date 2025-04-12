using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;

    void LateUpdate()
    {
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
