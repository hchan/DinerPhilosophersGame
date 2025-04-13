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
        if (GetImage().fillAmount > 0f)
        {
            GetImage().fillAmount -= Time.deltaTime / RunDinerPhilosophersGame.SIMULATION_TIME;
        }
    }

    public void UpdateTimer() {
        GetImage().fillAmount = 1f;
    }

    public Image GetImage() {
        return GameManager.Instance.Get("timerImage").GetComponent<Image>();
    }
}
