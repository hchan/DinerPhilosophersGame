using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTimer() {
        GameManager.Instance.Get("timerImage").GetComponent<Image>().fillAmount = 1f;
    }
}
