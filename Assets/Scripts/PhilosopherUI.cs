using UnityEngine;

public class PhilosopherUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float defaultAlpha = 0.25f;
    private float highlightAlpha = 0.5f;
    private float selectedAlpha = 1.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            SetAlpha(defaultAlpha);
        }
    }

    void OnMouseDown()
    {
        SetAlpha(selectedAlpha);
    }

    void OnMouseEnter()
    {
        SetAlpha(highlightAlpha);
    }

    void OnMouseExit()
    {
        SetAlpha(defaultAlpha);
    }

    void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
