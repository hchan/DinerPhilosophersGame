using UnityEngine;

public class PhilosopherUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    void Start()
    {
        // Access the GameManager singleton
        gameManager = GameManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        SetAlpha(gameManager.selectedAlpha);
    }

    void OnMouseEnter()
    {
        SetAlpha(gameManager.highlightAlpha);
    }

    void OnMouseExit()
    {
        SetAlpha(gameManager.defaultAlpha);
    }

    void SetAlpha(float alpha)
    {
        gameManager.SetAlpha(gameObject, alpha);
    }
}
