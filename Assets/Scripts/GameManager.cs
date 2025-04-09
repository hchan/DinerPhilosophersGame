using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
     public List<GameObject> chopsticks = new List<GameObject>();
    public List<GameObject> philosophers = new List<GameObject>();
    public float defaultAlpha = 0.50f;
    public float highlightAlpha = 0.75f;
    public float selectedAlpha = 1.0f;
    public static GameManager Instance
    {
        get
        {
            // If the instance is null, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // If no instance is found, create a new GameManager object
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    // Ensure the instance is only set once
    private void Awake()
    {
        // If an instance already exists, destroy this object (to maintain only one instance)
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the instance across scene changes
        }
    }

   

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager started");

        int chopstickLayer = LayerMask.NameToLayer("chopstick");
        int philosopherLayer = LayerMask.NameToLayer("philosopher");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Cache the chopstick and philosopher GameObjects
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == chopstickLayer)
            {
                chopsticks.Add(obj);
                Debug.Log($"Found chopstick: {obj.name}");
            }
            else if (obj.layer == philosopherLayer)
            {
                philosophers.Add(obj);
                Debug.Log($"Found philosopher: {obj.name}");
            }
        }

        // Set default alpha for each chopstick and philosopher
        foreach (GameObject chopstick in chopsticks)
        {
            SetAlpha(chopstick, defaultAlpha);
        }

        foreach (GameObject philosopher in philosophers)
        {
            SetAlpha(philosopher, defaultAlpha);
        }
    }

    public void SetAlpha(GameObject gameObject, float alpha)
    {
        if (gameObject == null) return;

        // Try to get the TextMeshPro component (3D text or UI text)
        var textMeshPro = gameObject.GetComponent<TextMeshPro>();
        if (textMeshPro != null)
        {
            SetTextAlpha(textMeshPro, alpha);
            return;
        }

        var textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUI != null)
        {
            SetTextAlpha(textMeshProUGUI, alpha);
            return;
        }

        // If it's not TextMeshPro or TextMeshProUGUI, try Renderer
        var renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            SetRendererAlpha(renderer, alpha);
        }
        else
        {
            Debug.LogWarning("No TextMeshPro or Renderer component found.");
        }
    }

    private void SetTextAlpha(MaskableGraphic textComponent, float alpha)
    {
        var color = textComponent.color;
        color.a = alpha;
        textComponent.color = color;
    }

    private void SetRendererAlpha(Renderer renderer, float alpha)
    {
        var color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }
}
