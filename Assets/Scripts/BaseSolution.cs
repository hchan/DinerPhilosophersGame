using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSolution : MonoBehaviour
{
    public GameObject baseSolution;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GlobalOrderSolution()
    {
        ExecuteSolution((runDinerPhilosophersGame, i) => runDinerPhilosophersGame.GlobalOrderSolution(i));
    }

    public void PartialOrderSolution()
    {
        ExecuteSolution((runDinerPhilosophersGame, i) => runDinerPhilosophersGame.PartialOrderSolution(i));
    }

    private void ExecuteSolution(System.Func<RunDinerPhilosophersGame, int, List<int>[]> solutionStrategy)
    {
        GameManager.Instance.Get("TabManager").GetComponent<TabManager>().ShowTab("diningTable");
        RunDinerPhilosophersGame runDinerPhilosophersGame = GameManager.Instance.Get("runDinerPhilosophersGame").GetComponent<RunDinerPhilosophersGame>();
        for (int i = 0; i < RunDinerPhilosophersGame.NUM_PHILOSOPHERS; i++)
        {
            GameManager.Instance.chopstickData[i].availablePickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.Clear();
            GameManager.Instance.chopstickData[i].availableDropChopsticks.Clear();
            GameManager.Instance.chopstickData[i].orderDropChopsticks.Clear();

            List<int>[] pickupAndDropLists = solutionStrategy(runDinerPhilosophersGame, i);
            GameManager.Instance.chopstickData[i].orderPickupChopsticks.AddRange(pickupAndDropLists[0]);
            GameManager.Instance.chopstickData[i].orderDropChopsticks.AddRange(pickupAndDropLists[1]);            
        }
        GameManager.Instance.Get("runButton").GetComponent<Button>().onClick.Invoke(); // Invoke the click event of the run button ... this includes ALL listeners
    }
}
