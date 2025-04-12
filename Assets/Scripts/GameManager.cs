using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;
    public string[] philosopherNames = { "Tigress", "Monkey", "Viper", "Crane", "Mantis" };

    public Dictionary<string, GameObject> objectCache = new Dictionary<string, GameObject>();
    public Dictionary<int, ChopstickData> chopstickData = new Dictionary<int, ChopstickData>();

    public int numPhilosophers = 5; // Number of philosophers
    public float defaultAlpha = 0.50f;
    public float highlightAlpha = 0.75f;
    public float fullAlpha = 1.0f;

    public int selectedPhilosopherId = -1; // -1 means no philosopher is selected

    public List<GameObject> chopsticksInChopsticksHolder = new List<GameObject>();
    
    // Ensure the instance is only set once
    private void Awake()
    {
        Instance = this;
        CacheAllObjects();
    }

    private void CacheAllObjects()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (!objectCache.ContainsKey(obj.name))
            {
                objectCache[obj.name] = obj;
            }
        }
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("chopsticksHolder");
        foreach (GameObject obj in objectsWithTag)
        {
            chopsticksInChopsticksHolder.Add(obj);
        }
    }

    public GameObject Get(string name)
    {
        return objectCache.TryGetValue(name, out var obj) ? obj : null;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager started");
        ResetAlpha();
//        Get("pickupChopsticksText").SetActive(false);
 //       Get("availablePickupChopsticksDropdown").SetActive(false);
        for (int i = 0; i < numPhilosophers; i++)
        {
            chopstickData[i] = new ChopstickData(GetRightChopstickId(i), GetLeftChopstickId(i));
        }
//        availablePickupChopsticksDropdown = Get("availablePickupChopsticksDropdown").GetComponent<TMP_Dropdown>();
  //      Debug.Log("availablePickupChopsticksDropdown: " + availablePickupChopsticksDropdown.options.Count);
    }

    public void ResetAlpha()
    {
        
        // loop through objectCache and set alpha to defaultAlpha
        foreach (var obj in objectCache.Values)
        {
            // check if obj is a gameobject
            if (obj.name.Contains("chopstick") || obj.name.Contains("philosopher"))
            {
                SetAlpha(obj, defaultAlpha);
            }
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


    public int GetLeftChopstickId(int philosopherId) {
        return (philosopherId+1) % numPhilosophers;
    }

    public int GetRightChopstickId(int philosopherId) {
        return philosopherId;
    }
}
