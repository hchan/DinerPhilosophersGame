using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunGlobalOrderSolution : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button runGlobalOrderSolution;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("Run Global Order Solution Button clicked");
        GameManager.Instance.Get("TabManager").GetComponent<TabManager>().ShowTab("diningTable");
        RunDinerPhilosophersGame runDinerPhilosophersGame = GameManager.Instance.Get("runDinerPhilosophersGame").GetComponent<RunDinerPhilosophersGame>();
        for (int i = 0; i < RunDinerPhilosophersGame.NUM_PHILOSOPHERS; i++)
        {
            GameManager.Instance.chopstickData[i].availablePickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].availableDropChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderDropChopsticks.Clear();

            List<int>[] pickupAndDropLists = runDinerPhilosophersGame.GlobalOrderSolution(i);
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.AddRange(pickupAndDropLists[0]);
            GameManager.Instance.chopstickData[i].orderDropChopsticks.AddRange(pickupAndDropLists[1]);            
        }
         GameManager.Instance.Get("runButton").GetComponent<Button>().onClick.Invoke(); // Invoke the click event of the run button ... this includes ALL listeners
    }
}
